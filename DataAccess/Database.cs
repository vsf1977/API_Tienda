using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using API_Tienda;
using Microsoft.Extensions.Configuration;

namespace API_Tienda.DataAccess
{
    //Clase para el manejo de la conexion con la base de datos
    public class Database
    {
        //Metodo para gestionar la conexion a la base de datos
        SqlConnection databaseConnection;
        public SqlConnection Conectar()
        {
            //Se consigue la cadena de conexion, guardada en el archivo appsettings.json
            string cadena = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("ConnectionStrings")["DefaultConnection"];
            databaseConnection = new SqlConnection(cadena);
            if (databaseConnection.State == System.Data.ConnectionState.Closed)
                databaseConnection.Open();
            return databaseConnection;
        }

        public void Desconectar()
        {
            if (databaseConnection.State == System.Data.ConnectionState.Open)
                databaseConnection.Close();
        }
    }
}
