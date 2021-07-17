using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using API_Tienda;
using Microsoft.Extensions.Configuration;

namespace API_Tienda.DataAccess
{
    public class Database
    {
        SqlConnection databaseConnection;
        public SqlConnection Conectar()
        {
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
