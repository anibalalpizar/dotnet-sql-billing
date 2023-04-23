using System;
using System.Configuration;
using System.Text;


class FactoryConexion
{

    public static string CreateConnection()
    {

        StringBuilder conexion = new StringBuilder();
        bool existe = false;

        // Validacion de la conexion
        if (ConfigurationManager.ConnectionStrings.Count == 0)
        {
            throw new Exception("No existen registradas ConnectionStrings en el archivo app.config, revise!");
        }

        for (int i = 0; i <= ConfigurationManager.ConnectionStrings.Count - 1; i++)
        {
            if (ConfigurationManager.ConnectionStrings[i].Name.Equals("default", StringComparison.InvariantCultureIgnoreCase))
                existe = true;
        }

        if (!existe)
        {
            throw new Exception("No existe registrada en ConnectionStrings del app.config el Key default!");
        }


        // Lee la conexion default
        conexion.AppendFormat("{0}", ConfigurationManager.ConnectionStrings["default"].ConnectionString);

        return conexion.ToString();
    }

    public static string CreateConnection(string pUsuario, String pContrasena)
    {

        StringBuilder conexion = new StringBuilder();
        bool existe = false;

        // Validacion de la conexion
        if (ConfigurationManager.ConnectionStrings.Count == 0)
        {
            throw new Exception("No existen registradas ConnectionStrings en el archivo app.config, revise!");
        }

        for (int i = 0; i < ConfigurationManager.ConnectionStrings.Count - 1; i++)
        {
            if (ConfigurationManager.ConnectionStrings[i].Name.Equals("default", StringComparison.InvariantCultureIgnoreCase))
                existe = true;
        }

        if (!existe)
        {
            throw new Exception("No existe registrada en ConnectionStrings del app.config el Key default!");
        }


        // Lee la conexion default
        conexion.AppendFormat("{0}", ConfigurationManager.ConnectionStrings["default"].ConnectionString);
        // Agrega al usuario
        conexion.AppendFormat("User Id={0};Password={1}", pUsuario, pContrasena);
        return conexion.ToString();
    }

    public static string CreateConnection(string pUsuario, String pContrasena, string pConexion)
    {

        StringBuilder conexion = new StringBuilder();
        bool existe = false;
        // Validacion de la conexion
        if (ConfigurationManager.ConnectionStrings.Count == 0)
        {
            throw new Exception("No existen registradas ConnectionStrings en el archivo app.confi, revise!");
        }

        for (int i = 0; i < ConfigurationManager.ConnectionStrings.Count - 1; i++)
        {
            if (ConfigurationManager.ConnectionStrings[i].Name.Equals(pConexion, StringComparison.InvariantCultureIgnoreCase))
                existe = true;
        }

        if (!existe)
        {
            throw new Exception("No existe registrada en ConnectionStrings del app.config el Key default!");
        }


        // Lee la conexion default
        conexion.AppendFormat("{0}", ConfigurationManager.ConnectionStrings[pConexion].ConnectionString);
        // Agrega al usuario
        conexion.AppendFormat("User Id={0};Password={1}", pUsuario, pContrasena);
        return conexion.ToString();
    }

}

