using dotnet_facturacion.Interfaces;
using dotnet_facturacion.Layers.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace dotnet_facturacion.Layers.DAL
{
    public class DALFactura : IDALFactura
    {
        public int GetCurrentNumeroFactura()
        {
            DataSet ds = null;
            IDbCommand command = new SqlCommand();
            int numeroFactura = 0;
            string sql = @"SELECT current_value FROM sys.sequences WHERE name = 'SequenceNoFactura'  ";
            DataTable dt = null;
            try
            {

                command.CommandText = sql;
                command.CommandType = CommandType.Text;

                using (IDataBase db = FactoryDatabase.CreateDataBase(FactoryConexion.CreateConnection()))
                {

                    ds = db.ExecuteReader(command, "query");
                }

                // Extraer la tabla 
                dt = ds.Tables[0];
                //Extraer el valor que viene en el DataTable 
                numeroFactura = int.Parse(dt.Rows[0][0].ToString());
                return numeroFactura;
            }
            catch (Exception er)
            {
                StringBuilder msg = new StringBuilder();
                if (er is SqlException)
                {
                    msg.AppendFormat("{0}\n", UtilError.CreateSQLExceptionsErrorDetails(MethodBase.GetCurrentMethod(), command, er as SqlException));
                    throw new CustomException(UtilError.GetCustomErrorByNumber(er as SqlException));
                }
                else
                {
                    msg.AppendFormat(UtilError.CreateGenericErrorExceptionDetail(MethodBase.GetCurrentMethod(), er));
                    throw;
                }
            }
        }

        public int GetNextNumeroFactura()
        {
            DataSet ds = null;
            IDbCommand command = new SqlCommand();
            int numeroFactura = 0;
            string sql = @"SELECT NEXT VALUE FOR SequenceNoFactura";

            DataTable dt = null;
            try
            {
                command.CommandText = sql;
                command.CommandType = CommandType.Text;

                using (IDataBase db = FactoryDatabase.CreateDataBase(FactoryConexion.CreateConnection()))
                {
                    ds = db.ExecuteReader(command, "query");
                }

                // Extraer la tabla 
                dt = ds.Tables[0];
                //Extraer el valor que viene en el DataTable 
                numeroFactura = int.Parse(dt.Rows[0][0].ToString());
                return numeroFactura;
            }
            catch (Exception er)
            {
                StringBuilder msg = new StringBuilder();
                if (er is SqlException)
                {
                    msg.AppendFormat("{0}\n", UtilError.CreateSQLExceptionsErrorDetails(MethodBase.GetCurrentMethod(), command, er as SqlException));
                    throw new CustomException(UtilError.GetCustomErrorByNumber(er as SqlException));
                }
                else
                {
                    msg.AppendFormat(UtilError.CreateGenericErrorExceptionDetail(MethodBase.GetCurrentMethod(), er));
                    throw;
                }
            }
        }

        public FacturaEncabezado SaveFactura(FacturaEncabezado pFactura)
        {
            FacturaEncabezado oFacturaEncabezado = null;
            string sqlEncabezado = string.Empty;
            string sqlDetalle = string.Empty;
            string sqlElectronico = string.Empty;
            SqlCommand cmdFacturaEncabezado = new SqlCommand();
            SqlCommand cmdFacturaDetalle = new SqlCommand();
            SqlCommand cmdElectronico = new SqlCommand();
            List<IDbCommand> listaCommands = new List<IDbCommand>();
            double rows = 0;

            sqlEncabezado = @"  
                            INSERT INTO [dbo].[FacturaEncabezado]
                                       ([IdFactura]
                                       ,[IdTarjeta]
                                       ,[FechaFacturacion]
                                       ,[IdCliente]
                                       ,[EstadoFactura]
                                       ,[TipoPago]
                                       ,TarjetaNumero )
                                 VALUES
                                        (@IdFactura
                                       ,@IdTarjeta
                                       ,getdate()
                                       ,@IdCliente
                                       ,@EstadoFactura
                                       ,@TipoPago
                                       ,@TarjetaNumero )  ";

            try
            {
                // Encabezado de factura
                cmdFacturaEncabezado.Parameters.AddWithValue("@IdFactura", pFactura.IdFactura);
                cmdFacturaEncabezado.Parameters.AddWithValue("@IdTarjeta", pFactura._Tarjeta.IdTarjeta);
                cmdFacturaEncabezado.Parameters.AddWithValue("@IdCliente", pFactura._Cliente.IdCliente);
                cmdFacturaEncabezado.Parameters.AddWithValue("@EstadoFactura", pFactura.EstadoFactura);
                cmdFacturaEncabezado.Parameters.AddWithValue("@TipoPago", pFactura.TipoPago);
                cmdFacturaEncabezado.Parameters.AddWithValue("@TarjetaNumero", pFactura.TarjetaNumero);
                cmdFacturaEncabezado.CommandText = sqlEncabezado;
                cmdFacturaEncabezado.CommandType = CommandType.Text;
                // Agregar a la lista de commands
                listaCommands.Add(cmdFacturaEncabezado);


                // Detalle de factura
                sqlDetalle = @" 
                    INSERT INTO[dbo].[FacturaDetalle]
                               ([IdFactura]
                               ,[Secuencia]
                               ,[IdElectronico]
                               ,[Cantidad]
                               ,[Precio]
                               ,[Impuesto])
                         VALUES
                               (@IdFactura
                               ,@Secuencia
                               ,@IdElectronico
                               ,@Cantidad
                               ,@Precio
                               ,@Impuesto) ";

                // Guardar el detalle de la factura y a la vez rebajar el saldo del producto en Electronico
                foreach (FacturaDetalle pFacturaDetalle in pFactura._ListaFacturaDetalle)
                {
                    cmdFacturaDetalle = new SqlCommand();
                    cmdFacturaDetalle.Parameters.AddWithValue("@IdFactura", pFacturaDetalle.IdFactura);
                    cmdFacturaDetalle.Parameters.AddWithValue("@Secuencia", pFacturaDetalle.Secuencia);
                    cmdFacturaDetalle.Parameters.AddWithValue("@IdElectronico", pFacturaDetalle.IdElectronico);
                    cmdFacturaDetalle.Parameters.AddWithValue("@Cantidad", pFacturaDetalle.Cantidad);
                    cmdFacturaDetalle.Parameters.AddWithValue("@Precio", pFacturaDetalle.Precio);
                    cmdFacturaDetalle.Parameters.AddWithValue("@Impuesto", pFacturaDetalle.Impuesto);
                    cmdFacturaDetalle.CommandText = sqlDetalle;
                    cmdFacturaDetalle.CommandType = CommandType.Text;
                    // Agregar a la lista de comandos
                    listaCommands.Add(cmdFacturaDetalle);

                    // Rebajar 
                    cmdElectronico = new SqlCommand();
                    cmdElectronico.Parameters.AddWithValue("@IdElectronico", pFacturaDetalle.IdElectronico);
                    cmdElectronico.Parameters.AddWithValue("@Cantidad", pFacturaDetalle.Cantidad);
                    sqlElectronico = @"Update Electronico 
                                       Set Cantidad =  Cantidad - @Cantidad 
                                       Where IdElectronico = @IdElectronico";
                    cmdElectronico.CommandText = sqlElectronico;
                    cmdElectronico.CommandType = CommandType.Text;
                    listaCommands.Add(cmdElectronico);

                }


                using (IDataBase db = FactoryDatabase.CreateDataBase(FactoryConexion.CreateConnection()))
                {

                    rows = db.ExecuteNonQuery(listaCommands, IsolationLevel.ReadCommitted);
                }

                // Si devuelve filas quiere decir que se salvo entonces extraerlo

                if (rows == 0)
                {
                    throw new Exception("No se pudo salvar correctamente la factura");
                }
                else
                {
                    oFacturaEncabezado = GetFactura(pFactura.IdFactura);
                }

                return oFacturaEncabezado;
            }
            catch (Exception er)
            {
                StringBuilder msg = new StringBuilder();
                if (er is SqlException)
                {
                    msg.AppendFormat("{0}\n", UtilError.CreateSQLExceptionsErrorDetails(MethodBase.GetCurrentMethod(), null, er as SqlException));
                    throw new CustomException(UtilError.GetCustomErrorByNumber(er as SqlException));
                }
                else
                {
                    msg.AppendFormat(UtilError.CreateGenericErrorExceptionDetail(MethodBase.GetCurrentMethod(), er));
                    throw;
                }
            }
        }
        private FacturaEncabezado GetFactura(double pNumeroFactura)
        {

            FacturaEncabezado oFacturaEncabezado = new FacturaEncabezado();
            DataSet ds = null;
            IDALCliente _DALCliente = new DALCliente();
            IDALTarjeta _DALTarjeta = new DALTarjeta();
            SqlCommand command = new SqlCommand();

            string sql = @"  ";


            sql = @"SELECT        FacturaEncabezado.IdFactura, FacturaEncabezado.IdTarjeta, FacturaEncabezado.IdCliente, FacturaEncabezado.FechaFacturacion, FacturaEncabezado.EstadoFactura, FacturaEncabezado.TipoPago, 
                                             FacturaEncabezado.TarjetaNumero, FacturaDetalle.Secuencia, FacturaDetalle.IdElectronico, FacturaDetalle.Cantidad, FacturaDetalle.Precio, FacturaDetalle.Impuesto
                    FROM            FacturaEncabezado INNER JOIN
                                             FacturaDetalle ON FacturaEncabezado.IdFactura = FacturaDetalle.IdFactura
                    WHERE        (FacturaEncabezado.IdFactura = @IdFactura) ";

            try
            {
                command.CommandText = sql;
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@IdFactura", pNumeroFactura);

                using (IDataBase db = FactoryDatabase.CreateDataBase(FactoryConexion.CreateConnection()))
                {
                    ds = db.ExecuteReader(command, "query");
                }

                // Si devolvió filas
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    oFacturaEncabezado = new FacturaEncabezado()
                    {
                        EstadoFactura = (bool)dr["EstadoFactura"],
                        FechaFacturacion = DateTime.Parse(dr["FechaFacturacion"].ToString()),
                        IdFactura = double.Parse(dr["IdFactura"].ToString()),
                        _Cliente = _DALCliente.GetClienteById(dr["IdCliente"].ToString()),
                        _Tarjeta = _DALTarjeta.GetTarjetaById(int.Parse(dr["IdTarjeta"].ToString())),
                        TipoPago = (int)dr["TipoPago"]
                    };

                    foreach (var item in ds.Tables[0].Rows)
                    {
                        FacturaDetalle oFacturaDetalle = new FacturaDetalle();
                        oFacturaDetalle.IdElectronico = double.Parse(dr["IdElectronico"].ToString());
                        oFacturaDetalle.Cantidad = int.Parse(dr["Cantidad"].ToString());
                        oFacturaDetalle.Precio = double.Parse(dr["Precio"].ToString());
                        oFacturaDetalle.IdFactura = double.Parse(dr["IdFactura"].ToString());
                        // Calcular el Impuesto
                        oFacturaDetalle.Impuesto = double.Parse(dr["Impuesto"].ToString());
                        // Enumerar la secuencia
                        oFacturaDetalle.Secuencia = int.Parse(dr["Secuencia"].ToString());
                        // Agregar
                        oFacturaEncabezado.AddDetalle(oFacturaDetalle);
                    }
                }
                return oFacturaEncabezado;
            }
            catch (Exception er)
            {
                StringBuilder msg = new StringBuilder();
                if (er is SqlException)
                {
                    msg.AppendFormat("{0}\n", UtilError.CreateSQLExceptionsErrorDetails(MethodBase.GetCurrentMethod(), command, er as SqlException));
                    throw new CustomException(UtilError.GetCustomErrorByNumber(er as SqlException));
                }
                else
                {
                    msg.AppendFormat(UtilError.CreateGenericErrorExceptionDetail(MethodBase.GetCurrentMethod(), er));
                    throw;
                }
            }
        }
        public double GetTotalFactura(double pNumeroFactura)
        {

            double sumatoria = 0d;
            string sql = @"  select sum(Cantidad * Precio + Impuesto) from FacturaDetalle  f
                             where f.IdFactura = @pNumeroFactura";
            SqlCommand command = new SqlCommand();
            command.CommandText = sql;
            command.CommandType = CommandType.Text;
            command.Parameters.AddWithValue("@pNumeroFactura", pNumeroFactura);

            using (IDataBase db = FactoryDatabase.CreateDataBase(FactoryConexion.CreateConnection()))
            {
                sumatoria = db.ExecuteScalar(command);
            }

            return sumatoria;
        }
    }
}
