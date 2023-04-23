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
    public class DALImpuesto : IDALImpuesto
    {
        public Impuesto GetImpuesto()
        {

            IDataReader reader = null;
            SqlCommand command = new SqlCommand();
            Impuesto oImpuesto = new Impuesto();
            string sql = @" select  * from Impuesto WITH (HOLDLock)    ";

            try
            {
                command.CommandText = sql;
                command.CommandType = CommandType.Text;

                using (IDataBase db = FactoryDatabase.CreateDataBase(FactoryConexion.CreateConnection()))
                {
                    reader = db.ExecuteReader(command);

                    while (reader.Read())
                    {
                        oImpuesto.Porcentaje = int.Parse(reader["porcentaje"].ToString());
                    }
                }


                return oImpuesto;
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
