using dotnet_facturacion.Interfaces;
using dotnet_facturacion.Layers.BLL;
using dotnet_facturacion.Layers.DAL;
using dotnet_facturacion.Layers.Entities;
using dotnet_facturacion.Layers.UI.Filtros;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace dotnet_facturacion.Layers.UI.Procesos
{
    public partial class frmFacturacion : Form
    {
        private Cliente _Cliente = null;
        private FacturaEncabezado _FacturaEncabezado = null;
        public frmFacturacion()
        {
            InitializeComponent();
        }

        private void frmFacturacion_Load(object sender, EventArgs e)
        {
            IBLLFactura _BLLFactura = new BLLFactura();
            try
            {
                // Mostar Numero de factura
                this.txtNumeroFactura.Text = _BLLFactura.GetCurrentNumeroFactura().ToString();

                // Cargar las Tarjetas
                CargarTarjeta();

            }
            catch (SqlException sqlError)
            {
                // Mensaje de Error
                MessageBox.Show("Se ha producido el siguiente error: \n" + UtilError.GetCustomErrorByNumber(sqlError), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception er)
            {

                StringBuilder msg = new StringBuilder();
                msg.AppendFormat(UtilError.CreateGenericErrorExceptionDetail(MethodBase.GetCurrentMethod(), er));
                // Mensaje de Error
                MessageBox.Show("Se ha producido el siguiente error: " + er.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void CargarTarjeta()
        {
            IBLLTarjeta _BLLTarjeta = new BLLTarjeta();
            foreach (var oTarjeta in _BLLTarjeta.GetAllTarjeta())
            {
                this.cmbTarjeta.Items.Add(oTarjeta);
            }
            cmbTarjeta.SelectedIndex = 0;
        }

        private void toolStripBtnNuevo_Click(object sender, EventArgs e)
        {
            IBLLFactura _BLLFactura = new BLLFactura();

            try
            {
                _Cliente = null;
                this.txtClienteId.Text = "";
                this.txtImpuesto.Text = "";
                this.txtNumeroTarjeta.Text = "";
                this.txtSubtotal.Text = "";
                this.txtTotal.Text = "";
                this.txtPrecio.Text = "";
                this.cmbTarjeta.SelectedIndex = 0;
                this.mskCantidad.Text = "";
                this.mskCodigoProduto.Text = "";
                this.txtNombreCliente.Text = "-";
                this.txtExistencia.Text = "";
                this.txtDescripcionElectronico.Text = "";
                this.rdbCredito_CheckedChanged(null, null);
                this.txtNumeroTarjeta.Text = "";
                txtNumeroTarjeta.Focus();
                _FacturaEncabezado = null;
                this.dgvDetalleFactura.Rows.Clear();
                // Mostar Numero de factura
                this.txtNumeroFactura.Text = _BLLFactura.GetNextNumeroFactura().ToString();
            }
            catch (SqlException sqlError)
            {
                // Mensaje de Error
                MessageBox.Show("Se ha producido el siguiente error: \n" + UtilError.GetCustomErrorByNumber(sqlError), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception er)
            {

                StringBuilder msg = new StringBuilder();
                msg.AppendFormat(UtilError.CreateGenericErrorExceptionDetail(MethodBase.GetCurrentMethod(), er));
                // Mensaje de Error
                MessageBox.Show("Se ha producido el siguiente error: " + er.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void rdbCredito_CheckedChanged(object sender, EventArgs e)
        {
            this.txtNumeroTarjeta.Text = "";
            this.cmbTarjeta.Enabled = true;
            txtNumeroTarjeta.Enabled = true;
        }

        private void toolStripBtnFacturar_Click(object sender, EventArgs e)
        {
            IBLLFactura _BLLFactura = new BLLFactura();
            string rutaImagen = @"c:\temp\qr.png";
            double numeroFactura = 0d;
            try
            {
                if (_Cliente == null)
                {
                    MessageBox.Show("Debe Seleccionar un Cliente", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.txtClienteId.Focus();
                    return;
                }

                if (_FacturaEncabezado == null)
                {
                    MessageBox.Show("No hay datos por facturar", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (_FacturaEncabezado._ListaFacturaDetalle.Count == 0)
                {
                    MessageBox.Show("No hay items en la factura ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                _FacturaEncabezado = _BLLFactura.SaveFactura(_FacturaEncabezado);

                numeroFactura = _FacturaEncabezado.IdFactura;

                if (_FacturaEncabezado == null)
                    MessageBox.Show("Error al crear factura!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                    toolStripBtnNuevo_Click(null, null);


                // Si existe borrelo
                if (File.Exists(rutaImagen))
                    File.Delete(rutaImagen);

                // Crear imagen quickresponse
                Image quickResponseImage = QuickResponse.QuickResponseGenerador(numeroFactura.ToString(), 53);

                // Salvarla en c:\temp para luego ser leida
                quickResponseImage.Save(rutaImagen, ImageFormat.Png);

              //  ofrmReporteFactura.ShowDialog();
            }
            catch (Exception er)
            {
                StringBuilder msg = new StringBuilder();
                msg.AppendFormat(UtilError.CreateGenericErrorExceptionDetail(MethodBase.GetCurrentMethod(), er));
                MessageBox.Show("Se ha producido el siguiente error: " + er.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnBuscarCliente_Click(object sender, EventArgs e)
        {
            frmFiltroCliente ofrmFiltroCliente = new frmFiltroCliente();
            IBLLCliente _BLLCliente = new BLLCliente();
            try
            {
                erpError.Clear();

                if (!string.IsNullOrEmpty(this.txtClienteId.Text))
                {
                    _Cliente = _BLLCliente.GetClienteById(this.txtClienteId.Text);
                }
                else
                {
                    // Mostrar ventan de filtro
                    ofrmFiltroCliente.ShowDialog();
                    if (ofrmFiltroCliente.DialogResult == DialogResult.OK)
                    {
                        _Cliente = ofrmFiltroCliente._Cliente;
                    }
                }

                if (_Cliente == null)
                {
                    MessageBox.Show("No existe el Cliente", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }


                if (this.rdbCredito.Checked && string.IsNullOrEmpty(this.txtNumeroTarjeta.Text))
                {
                    txtNumeroTarjeta.Focus();
                    erpError.SetError(txtNumeroTarjeta, "El pago  por tarjeta debe indicar su número");
                    MessageBox.Show("El pago es por tarjeta debe indicar su número", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }


                _FacturaEncabezado = new FacturaEncabezado()
                {
                    EstadoFactura = true,
                    FechaFacturacion = DateTime.Now,
                    IdFactura = double.Parse(this.txtNumeroFactura.Text),
                    _Cliente = _Cliente,
                    _Tarjeta = cmbTarjeta.SelectedItem as Tarjeta,
                    TipoPago = this.rdbContado.Checked ? 1 : 2,
                    TarjetaNumero = this.txtNumeroTarjeta.Text
                };



                this.txtNombreCliente.Text = _Cliente.ToString();
                this.txtClienteId.Text = _Cliente.IdCliente;
            }
            catch (SqlException sqlError)
            {
                // Mensaje de Error
                MessageBox.Show("Se ha producido el siguiente error: \n" + UtilError.GetCustomErrorByNumber(sqlError), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception er)
            {

                StringBuilder msg = new StringBuilder();
                msg.AppendFormat(UtilError.CreateGenericErrorExceptionDetail(MethodBase.GetCurrentMethod(), er));
                // Mensaje de Error
                MessageBox.Show("Se ha producido el siguiente error: " + er.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnBuscarProducto_Click(object sender, EventArgs e)
        {
            frmFiltroElectronico ofrmFiltroElectronico = new frmFiltroElectronico();
            Electronico oElectronico = null;
            try
            {
                ofrmFiltroElectronico.ShowDialog();

                if (ofrmFiltroElectronico.DialogResult == DialogResult.OK)
                {
                    oElectronico = ofrmFiltroElectronico._Electronico;
                    txtDescripcionElectronico.Text = oElectronico.DescripcionElectronico;
                    this.mskCodigoProduto.Text = oElectronico.IdElectronico.ToString();
                    this.txtPrecio.Text = oElectronico.Precio.ToString();
                    this.txtExistencia.Text = oElectronico.Cantidad.ToString();
                    this.mskCantidad.Focus();
                }
            }
            catch (Exception er)
            {
                StringBuilder msg = new StringBuilder();
                msg.AppendFormat(UtilError.CreateGenericErrorExceptionDetail(MethodBase.GetCurrentMethod(), er));
                MessageBox.Show("Se ha producido el siguiente error: " + er.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            IBLLImpuesto _BLLImpuesto = new BLLImpuesto();
            IBLLElectronico _BLLElectronico = new BLLElectronico();
            FacturaDetalle oFacturaDetalle = new FacturaDetalle();

            try
            {
                erpError.Clear();

                if (_FacturaEncabezado == null)
                {
                    MessageBox.Show("Debe agregar los datos del encabezado de la factura para continuar", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Validar que el Producto ya no se haya agregado
                if (_FacturaEncabezado._ListaFacturaDetalle.Count > 0)
                {
                    // Si ya se agrego no permitir agregarlo nuevamente
                    if (_FacturaEncabezado._ListaFacturaDetalle.FindAll(p => p.IdElectronico == double.Parse(mskCodigoProduto.Text)).Count > 0)
                    {
                        MessageBox.Show("El producto ya fue agregado previamente.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                if (string.IsNullOrEmpty(this.mskCantidad.Text))
                {
                    mskCantidad.Focus();
                    erpError.SetError(mskCantidad, "Debe ingresar la cantidad de artículos");
                    return;
                }


                if (double.Parse(this.mskCantidad.Text) <= 0)
                {
                    mskCantidad.Focus();
                    erpError.SetError(mskCantidad, "La cantidad debe ser mayor a CERO");
                    return;
                }

                // Valida que exista disponibilidad en el Inventario
                Electronico oElectronico = _BLLElectronico.AvabilityStock(double.Parse(this.mskCodigoProduto.Text),
                                                                          double.Parse(this.mskCantidad.Text));
                // Vuelve a mostrar Existencia.
                this.txtExistencia.Text = oElectronico.Cantidad.ToString();

                oFacturaDetalle.IdElectronico = double.Parse(this.mskCodigoProduto.Text);
                oFacturaDetalle.Cantidad = int.Parse(this.mskCantidad.Text);
                oFacturaDetalle.Precio = double.Parse(this.txtPrecio.Text);
                oFacturaDetalle.IdFactura = _FacturaEncabezado.IdFactura;
                // Calcular el Impuesto
                oFacturaDetalle.Impuesto = ((double)_BLLImpuesto.GetImpuesto().Porcentaje / 100D) * oFacturaDetalle.Precio * oFacturaDetalle.Cantidad;
                // Enumerar la secuencia
                oFacturaDetalle.Secuencia = _FacturaEncabezado._ListaFacturaDetalle.Count == 0 ?
                                                 1 : _FacturaEncabezado._ListaFacturaDetalle.Max(p => p.Secuencia) + 1;
                // Agregar
                _FacturaEncabezado.AddDetalle(oFacturaDetalle);


                string[] lineaFactura = {oFacturaDetalle.Secuencia.ToString(),
                                         this.mskCantidad.Text,
                                         this.txtDescripcionElectronico.Text,
                                         oFacturaDetalle.Cantidad.ToString(),
                                         oFacturaDetalle.Precio.ToString(),
                                         (oFacturaDetalle.Cantidad * oFacturaDetalle.Precio).ToString()
                                         };

                this.dgvDetalleFactura.Rows.Add(lineaFactura);

                this.txtSubtotal.Text = _FacturaEncabezado.GetSubTotal().ToString();
                this.txtImpuesto.Text = _FacturaEncabezado.GetImpuesto().ToString();
                this.txtTotal.Text = (_FacturaEncabezado.GetSubTotal() + _FacturaEncabezado.GetImpuesto()).ToString();

                this.mskCantidad.Text = "";
                this.txtDescripcionElectronico.Text = "";
                this.txtExistencia.Text = "";
                this.mskCodigoProduto.Text = "";
                mskCodigoProduto.Focus();
            }
            catch (Exception er)
            {
                StringBuilder msg = new StringBuilder();
                msg.AppendFormat(UtilError.CreateGenericErrorExceptionDetail(MethodBase.GetCurrentMethod(), er));
                MessageBox.Show("Se ha producido el siguiente error: " + er.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
