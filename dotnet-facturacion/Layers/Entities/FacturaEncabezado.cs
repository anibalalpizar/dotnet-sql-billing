using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotnet_facturacion.Layers.Entities
{
    public class FacturaEncabezado
    {
        public double IdFactura { set; get; }
        public Tarjeta _Tarjeta { set; get; }
        public string TarjetaNumero { set; get; }
        public DateTime FechaFacturacion { set; get; }
        public Cliente _Cliente { set; get; }
        public bool EstadoFactura { set; get; }
        public int TipoPago { set; get; }

        public List<FacturaDetalle> _ListaFacturaDetalle { get; } = new List<FacturaDetalle>();

        public void AddDetalle(FacturaDetalle pFacturaDetalle)
        {
            _ListaFacturaDetalle.Add(pFacturaDetalle);
        }

        public double GetSubTotal()
        {
            return _ListaFacturaDetalle.Sum(p => p.Cantidad * p.Precio);
        }

        public double GetImpuesto()
        {
            return _ListaFacturaDetalle.Sum(p => p.Impuesto);
        }
    }
}
