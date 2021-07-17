using System.Collections.Generic;
using System.Data.SqlClient;
using API_Tienda.DataAccess;

namespace Restaurante_API.Models
{
    public class Cliente
    {
        SqlCommand comando;
        SqlDataReader cursor;
        Database database = new Database();
        public string IdCliente { get; set; }
        public string nombre { get; set; }
        public string apellidos { get; set; }
        public string direccion { get; set; }
        public string telefono { get; set; }

        public List<Cliente> list()
        {
            comando = new SqlCommand("select * from cliente", database.Conectar());
            cursor = comando.ExecuteReader();
            List<Cliente> Clientes = new List<Cliente>();
            if (cursor.HasRows)
            {
                while (cursor.Read())
                {
                    Cliente Cliente = new Cliente();
                    Cliente.IdCliente = cursor.GetString(0);
                    Cliente.nombre = cursor.GetString(1);
                    Cliente.apellidos = cursor.GetString(2);
                    Cliente.direccion = cursor.GetString(3);
                    Cliente.telefono = cursor.GetString(4);
                    Clientes.Add(Cliente);
                }
            }
            else
            {
                Clientes = null;
            }
            database.Desconectar();
            return Clientes;
        }
        public Cliente GetCliente(string id)
        {
            comando = new SqlCommand(null, database.Conectar());
            comando.CommandText = "select * from cliente where IDcliente = @id";

            SqlParameter IDclienteParam = new SqlParameter("@id", System.Data.SqlDbType.VarChar, 20);

            IDclienteParam.Value = id;

            comando.Parameters.Add(IDclienteParam);
            comando.Prepare();
            Cliente Cliente = new Cliente();
            cursor = comando.ExecuteReader();

            if (cursor.HasRows)
            {
                cursor.Read();
                Cliente.IdCliente = cursor.GetString(0);
                Cliente.nombre = cursor.GetString(1);
                Cliente.apellidos = cursor.GetString(2);
                Cliente.direccion = cursor.GetString(3);
                Cliente.telefono = cursor.GetString(4);
            }
            else
                Cliente = null;
            database.Desconectar();
            return Cliente;
        }

        public void insertar()
        {
            comando = new SqlCommand(null, database.Conectar());
            comando.CommandText = "insert into cliente values (@IDcliente, @nombre, @apellidos, @direccion, @telefono)";

            SqlParameter IDclienteParam = new SqlParameter("@IDcliente", System.Data.SqlDbType.VarChar, 20);
            SqlParameter nombreParam = new SqlParameter("@nombre", System.Data.SqlDbType.VarChar, 30);
            SqlParameter apellidosParam = new SqlParameter("@apellidos", System.Data.SqlDbType.VarChar,40);
            SqlParameter direccionParam = new SqlParameter("@direccion", System.Data.SqlDbType.VarChar, 50);
            SqlParameter telefonoParam = new SqlParameter("@telefono", System.Data.SqlDbType.VarChar, 20);

            IDclienteParam.Value = IdCliente;
            nombreParam.Value = nombre;
            apellidosParam.Value = apellidos;
            direccionParam.Value = direccion;
            telefonoParam.Value  = telefono;

            comando.Parameters.Add(IDclienteParam);
            comando.Parameters.Add(nombreParam);
            comando.Parameters.Add(apellidosParam);
            comando.Parameters.Add(direccionParam);
            comando.Parameters.Add(telefonoParam);

            comando.Prepare();
            comando.ExecuteNonQuery();
            database.Desconectar();
        }

        public void actualizar()
        {
            comando = new SqlCommand(null, database.Conectar());
            comando.CommandText = "update cliente set nombre = @nombre, apellidos = @apellidos, direccion = @direccion, telefono = @telefono where IDcliente = @IDcliente";

            SqlParameter IDclienteParam = new SqlParameter("@IDcliente", System.Data.SqlDbType.VarChar, 20);
            SqlParameter nombreParam = new SqlParameter("@nombre", System.Data.SqlDbType.VarChar, 30);
            SqlParameter apellidosParam = new SqlParameter("@apellidos", System.Data.SqlDbType.VarChar, 40);
            SqlParameter direccionParam = new SqlParameter("@direccion", System.Data.SqlDbType.VarChar, 50);
            SqlParameter telefonoParam = new SqlParameter("@telefono", System.Data.SqlDbType.VarChar, 20);

            IDclienteParam.Value = IdCliente;
            nombreParam.Value = nombre;
            apellidosParam.Value = apellidos;
            direccionParam.Value = direccion;
            telefonoParam.Value = telefono;

            comando.Parameters.Add(IDclienteParam);
            comando.Parameters.Add(nombreParam);
            comando.Parameters.Add(apellidosParam);
            comando.Parameters.Add(direccionParam);
            comando.Parameters.Add(telefonoParam);

            comando.Prepare();
            comando.ExecuteNonQuery();
            database.Desconectar();
        }
        

        public void borrar()
        {
            comando = new SqlCommand(null, database.Conectar());
            comando.CommandText = "delete from cliente where IDcliente = @IDcliente";
            SqlParameter IDclienteParam = new SqlParameter("@IDcliente", System.Data.SqlDbType.VarChar, 10);
            IDclienteParam.Value = IdCliente;            
            comando.Parameters.Add(IDclienteParam);           
            comando.Prepare();
            comando.ExecuteNonQuery();
            database.Desconectar();
        }
    }
}
