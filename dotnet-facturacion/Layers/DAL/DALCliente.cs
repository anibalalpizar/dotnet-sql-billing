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
    public class DALCliente : IDALCliente
    {
        public async Task<bool> DeleteCliente(string pId)
        {
            bool retorno = false;
            double rows = 0d;
            SqlCommand command = new SqlCommand();
            try
            {
                string sql = @"Delete from  Cliente Where (IdCliente = @IdCliente) ";

                command.Parameters.AddWithValue("@IdCliente", pId);
                command.CommandText = sql;
                command.CommandType = CommandType.Text;

                using (IDataBase db = FactoryDatabase.CreateDataBase(FactoryConexion.CreateConnection()))
                {
                    rows = await db.ExecuteNonQueryAsync(command, IsolationLevel.ReadCommitted);
                }

                // Si devuelve filas quiere decir que se salvo entonces extraerlo
                if (rows > 0)
                    retorno = true;

                return retorno;
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

        public async Task<IEnumerable<Cliente>> GetAllClienteX()
        {

            List<Cliente> lista = new List<Cliente>();
            SqlCommand command = new SqlCommand();

            try
            {
                string sql = @" select * from  Cliente WITH (NOLOCK)  ";

                command.CommandText = sql;
                command.CommandType = CommandType.Text;

                using (IDataBase db = FactoryDatabase.CreateDataBase(FactoryConexion.CreateConnection()))
                {

                    DataTable dt = await db.ExecuteReaderAsync(command, "T");

                    // Iterar en todas las filas 
                    foreach (DataRow reader in dt.Rows)
                    {

                        // Mapearlas
                        Cliente oCliente = new Cliente();
                        oCliente.IdCliente = reader["IdCliente"].ToString();
                        oCliente.Nombre = reader["Nombre"].ToString();
                        oCliente.Apellido1 = reader["Apellido1"].ToString();
                        oCliente.Apellido2 = reader["Apellido2"].ToString();
                        oCliente.FechaNacimiento = DateTime.Parse(reader["FechaNacimiento"].ToString());
                        oCliente.IdProvincia = (int)reader["IdProvincia"];
                        oCliente.Sexo = (int)reader["Sexo"];

                        lista.Add(oCliente);
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

        public async Task<IEnumerable<Cliente>> GetAllCliente()
        {

            List<Cliente> lista = new List<Cliente>();
            SqlCommand command = new SqlCommand();

            try
            {
                string sql = @" select * from  Cliente WITH (NOLOCK)  ";

                command.CommandText = sql;
                command.CommandType = CommandType.Text;

                using (IDataBase db = FactoryDatabase.CreateDataBase(FactoryConexion.CreateConnection()))
                {
                    using (SqlDataReader reader = await db.ExecuteReaderAsync(command))
                    {
                        // Iterar en todas las filas 
                        while (await reader.ReadAsync())
                        {

                            // Mapearlas
                            Cliente oCliente = new Cliente();
                            oCliente.IdCliente = reader["IdCliente"].ToString();
                            oCliente.Nombre = reader["Nombre"].ToString();
                            oCliente.Apellido1 = reader["Apellido1"].ToString();
                            oCliente.Apellido2 = reader["Apellido2"].ToString();
                            oCliente.FechaNacimiento = DateTime.Parse(reader["FechaNacimiento"].ToString());
                            oCliente.IdProvincia = (int)reader["IdProvincia"];
                            oCliente.Sexo = (int)reader["Sexo"];

                            lista.Add(oCliente);
                        }
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

        public List<Cliente> GetClienteByFilter(string pDescripcion)
        {
            DataSet ds = null;
            List<Cliente> lista = new List<Cliente>();
            SqlCommand command = new SqlCommand();

            try
            {
                string sql = @" select * from  Cliente WITH (NOLOCK) Where Nombre+Apellido1+Apellido2 like @filtro ";
                command.Parameters.AddWithValue("@filtro", pDescripcion);
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
                        Cliente oCliente = new Cliente();
                        oCliente.IdCliente = dr["IdCliente"].ToString();
                        oCliente.Nombre = dr["Nombre"].ToString();
                        oCliente.Apellido1 = dr["Apellido1"].ToString();
                        oCliente.Apellido2 = dr["Apellido2"].ToString();
                        oCliente.FechaNacimiento = DateTime.Parse(dr["FechaNacimiento"].ToString());
                        oCliente.IdProvincia = (int)dr["IdProvincia"];
                        oCliente.Sexo = (int)dr["Sexo"];

                        lista.Add(oCliente);
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

        public Cliente GetClienteById(string pIdCliente)
        {
            DataSet ds = null;
            Cliente oCliente = null;
            SqlCommand command = new SqlCommand();

            try
            {
                string sql = @" select * from  Cliente Where IdCliente = @IdCliente ";
                command.Parameters.AddWithValue("@IdCliente", pIdCliente);
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
                        oCliente = new Cliente();
                        oCliente.IdCliente = dr["IdCliente"].ToString();
                        oCliente.Nombre = dr["Nombre"].ToString();
                        oCliente.Apellido1 = dr["Apellido1"].ToString();
                        oCliente.Apellido2 = dr["Apellido2"].ToString();
                        oCliente.FechaNacimiento = DateTime.Parse(dr["FechaNacimiento"].ToString());
                        oCliente.IdProvincia = (int)dr["IdProvincia"];
                        oCliente.Sexo = (int)dr["Sexo"];
                    }
                }

                return oCliente;
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


        public async Task<Cliente> SaveCliente(Cliente pCliente)
        {
            Cliente oCliente = null;
            SqlCommand command = new SqlCommand();
            // Insert
            string sql = @"Insert into Cliente(IdCliente,Nombre,Apellido1,Apellido2,Sexo,FechaNacimiento,IdProvincia)
                            values (@IdCliente,@Nombre,@Apellido1,@Apellido2,@Sexo,@FechaNacimiento,@IdProvincia)";
            try
            {
                command.Parameters.AddWithValue("@IdCliente", pCliente.IdCliente);
                command.Parameters.AddWithValue("@Nombre", pCliente.Nombre);
                command.Parameters.AddWithValue("@Apellido1", pCliente.Apellido1);
                command.Parameters.AddWithValue("@Apellido2", pCliente.Apellido2);
                command.Parameters.AddWithValue("@Sexo", pCliente.Sexo);
                command.Parameters.AddWithValue("@FechaNacimiento", pCliente.FechaNacimiento);
                command.Parameters.AddWithValue("@IdProvincia", pCliente.IdProvincia);
                command.CommandText = sql;
                command.CommandType = CommandType.Text;

                using (IDataBase db = FactoryDatabase.CreateDataBase(FactoryConexion.CreateConnection()))
                {
                    var rows = await db.ExecuteNonQueryAsync(command, IsolationLevel.ReadCommitted);

                    // Si devuelve filas quiere decir que se salvo entonces extraerlo
                    if (rows > 0)
                        oCliente = this.GetClienteById(pCliente.IdCliente);
                }

                return oCliente;

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

        public async Task<Cliente> UpdateCliente(Cliente pCliente)
        {

            string sql = @"Update Cliente SET IdCliente = @IdCliente, Nombre = @Nombre, Apellido1 = @Apellido1, Apellido2 = @Apellido2, Sexo = @Sexo, FechaNacimiento = @FechaNacimiento, IdProvincia = @IdProvincia  Where (IdCliente = @IdCliente) ";
            int rows = 0;
            SqlCommand command = new SqlCommand();
            Cliente oCliente = new Cliente();
            try
            {
                command.Parameters.AddWithValue("@IdCliente", pCliente.IdCliente);
                command.Parameters.AddWithValue("@Nombre", pCliente.Nombre);
                command.Parameters.AddWithValue("@Apellido1", pCliente.Apellido1);
                command.Parameters.AddWithValue("@Apellido2", pCliente.Apellido2);
                command.Parameters.AddWithValue("@Sexo", pCliente.Sexo);
                command.Parameters.AddWithValue("@FechaNacimiento", pCliente.FechaNacimiento);
                command.Parameters.AddWithValue("@IdProvincia", pCliente.IdProvincia);
                command.CommandText = sql;
                command.CommandType = CommandType.Text;

                using (IDataBase db = FactoryDatabase.CreateDataBase(FactoryConexion.CreateConnection()))
                {
                    rows = await db.ExecuteNonQueryAsync(command, IsolationLevel.ReadCommitted);
                }

                // Si devuelve filas quiere decir que se salvo entonces extraerlo
                if (rows > 0)
                    oCliente = this.GetClienteById(pCliente.IdCliente);

                return oCliente;

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
