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
    public class DALTarjeta : IDALTarjeta
    {
        public List<Tarjeta> GetAllTarjeta()
        {
            IDataReader reader = null;
            List<Tarjeta> lista = new List<Tarjeta>();
            Tarjeta oTarjeta = null;
            SqlCommand command = new SqlCommand();

            try
            {
                string sql = @" select * from  Tarjeta  With (NoLock)";
                command.CommandText = sql;
                command.CommandType = CommandType.Text;

                using (IDataBase db = FactoryDatabase.CreateDataBase(FactoryConexion.CreateConnection()))
                {
                    reader = db.ExecuteReader(command);
                    // Si devolvió filas
                    while (reader.Read())
                    {
                        oTarjeta = new Tarjeta();
                        oTarjeta.IdTarjeta = int.Parse(reader["IdTarjeta"].ToString());
                        oTarjeta.DescripcionTarjeta = reader["DescripcionTarjeta"].ToString();
                        lista.Add(oTarjeta);
                    }
                }
                return lista;

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


        public Tarjeta GetTarjetaById(int pIdTarjeta)
        {
            DataSet ds = null;
            Tarjeta oTarjeta = null;
            SqlCommand command = new SqlCommand();
            string sql = @" select * from  Tarjeta  With (NoLock)
                               where IdTarjeta = @idTarjeta";

            try
            {
                command.Parameters.AddWithValue("@idTarjeta", pIdTarjeta);
                command.CommandText = sql;
                command.CommandType = CommandType.Text;

                using (IDataBase db = FactoryDatabase.CreateDataBase(FactoryConexion.CreateConnection()))
                {
                    ds = db.ExecuteReader(command, "query");
                }

                // Si devolvió filas
                if (ds.Tables[0].Rows.Count > 0)
                {
                    // Iterar en todas las filas y Mapearlas
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        oTarjeta = new Tarjeta();
                        oTarjeta.IdTarjeta = int.Parse(dr["IdTarjeta"].ToString());
                        oTarjeta.DescripcionTarjeta = dr["DescripcionTarjeta"].ToString();

                    }
                }

                return oTarjeta;
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
    }
}
