using API_Tienda.DataAccess;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;

namespace API_Tienda.Models
{
    //Clase para gestionar el manejo de la data de los productos
    public class Producto
    {
        SqlCommand comando;
        SqlDataReader cursor;
        Database database = new Database();
        public int idProducto { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public decimal precio { get; set; }
        public byte descuento { get; set; }
        public string pais { get; set; }
        public Imagen[] imagenes { get; set; }

        //Metodo para listar todos los productos
        public List<Producto> GetAll()
        {
            comando = new SqlCommand("select * from producto", database.Conectar());
            cursor = comando.ExecuteReader();
            List<Producto> productos = new List<Producto>();
            string folderName = Path.Combine("Assets", "Images");
            while (cursor.Read())
            {
                Producto producto = new Producto();
                producto.idProducto = cursor.GetInt32(0);
                producto.nombre = cursor.GetString(1);
                producto.descripcion = cursor.GetString(2);
                producto.precio = cursor.GetDecimal(3);
                producto.descuento = cursor.GetByte(4);
                producto.pais = cursor.GetString(5);
                string imagePath = Path.Combine(Directory.GetCurrentDirectory(), folderName, producto.idProducto.ToString());
                //Se revisa si existe la carpeta con el nombre del id del producto para cargar las imagenes en cada´producto
                if (Directory.Exists(imagePath))
                {
                    if (Directory.GetFiles(imagePath).Length > 0)
                    {
                        int i = 0;
                        producto.imagenes = new Imagen[Directory.GetFiles(imagePath).Length];
                        foreach (var file in Directory.GetFiles(imagePath))
                        {
                            Imagen imagen = new Imagen();
                            imagen.nombre = file;
                            producto.imagenes[i] = imagen;
                            i++;
                        }
                    }
                        
                }
                productos.Add(producto);
            }
            database.Desconectar();
            return productos;
        }

        //Metodo para gestionar las consultas de uno o varios parametros con la base de datos.
        public List<Producto> list(string? nombre, decimal? preciomax, decimal? preciomin, byte? descuento, string? pais, string query_string)
        {
            //Se crea parametros para evitar el sql injection, dependiendo de los parametros pasados en el request
            comando = new SqlCommand(query_string, database.Conectar());
            if (query_string.IndexOf("nombre") >= 0)
            {
                SqlParameter nombreParam = new SqlParameter("@nombre", System.Data.SqlDbType.VarChar, 50);
                nombreParam.Value = nombre;
                comando.Parameters.Add(nombreParam);
            }
            if (query_string.IndexOf("pais") >= 0)
            {
                SqlParameter paisParam = new SqlParameter("@pais", System.Data.SqlDbType.VarChar, 100);
                paisParam.Value = pais;
                comando.Parameters.Add(paisParam);
            }
            if (query_string.IndexOf("precio") >= 0)
            {
                SqlParameter preciomaxParam = new SqlParameter("@preciomax", System.Data.SqlDbType.Decimal);
                preciomaxParam.Value = preciomax;
                preciomaxParam.Precision = 10;
                preciomaxParam.Scale = 3;
                comando.Parameters.Add(preciomaxParam);
                SqlParameter preciominParam = new SqlParameter("@preciomin", System.Data.SqlDbType.Decimal);
                preciominParam.Value = preciomin;
                preciominParam.Precision = 10;
                preciominParam.Scale = 3;
                comando.Parameters.Add(preciominParam);
            }
            if (query_string.IndexOf("descuento") >= 0)
            {
                SqlParameter descuentoParam = new SqlParameter("@descuento", System.Data.SqlDbType.TinyInt);
                descuentoParam.Value = descuento;
                comando.Parameters.Add(descuentoParam);
            }
            comando.Prepare();
            cursor = comando.ExecuteReader();
            List<Producto> productos = new List<Producto>();
            if (cursor.HasRows)
            {
                string folderName = Path.Combine("Assets", "Images");
                while (cursor.Read())
                {
                    Producto producto = new Producto();
                    producto.idProducto = cursor.GetInt32(0);
                    producto.nombre = cursor.GetString(1);
                    producto.descripcion = cursor.GetString(2);
                    producto.precio = cursor.GetDecimal(3);
                    producto.descuento = cursor.GetByte(4);
                    producto.pais = cursor.GetString(5);
                    string imagePath = Path.Combine(Directory.GetCurrentDirectory(), folderName, producto.idProducto.ToString());
                    //Se revisa si existe la carpeta con el nombre del id del producto para cargar las imagenes en cada´producto
                    if (Directory.Exists(imagePath))
                    {
                        if (Directory.GetFiles(imagePath).Length > 0)
                        {
                            int i = 0;
                            producto.imagenes = new Imagen[Directory.GetFiles(imagePath).Length];
                            foreach (var file in Directory.GetFiles(imagePath))
                            {
                                Imagen imagen = new Imagen();
                                imagen.nombre = file;
                                producto.imagenes[i] = imagen;
                                i++;
                            }
                        }

                    }
                    productos.Add(producto);
                }
            }
            database.Desconectar();
            return productos;
        }

        //Metodo para gestionar la insercion de un producto en la base de datos
        public void insertar()
        {
            comando = new SqlCommand(null, database.Conectar());
            comando.CommandText = "insert into producto values (@nombre, @desc, @precio, @descuento, @pais)";

            SqlParameter nombreParam = new SqlParameter("@nombre", System.Data.SqlDbType.VarChar, 50);
            SqlParameter descParam = new SqlParameter("@desc", System.Data.SqlDbType.VarChar, 100);
            SqlParameter precioParam = new SqlParameter("@precio", System.Data.SqlDbType.Decimal);
            SqlParameter descuentoParam = new SqlParameter("@descuento", System.Data.SqlDbType.TinyInt);
            SqlParameter paisParam = new SqlParameter("@pais", System.Data.SqlDbType.VarChar, 50);

            nombreParam.Value = nombre;
            descParam.Value = descripcion;
            precioParam.Value = precio;
            precioParam.Precision = 10;
            precioParam.Scale = 3;
            descuentoParam.Value = descuento;
            paisParam.Value = pais;

            comando.Parameters.Add(nombreParam);
            comando.Parameters.Add(descParam);
            comando.Parameters.Add(precioParam);
            comando.Parameters.Add(descuentoParam);
            comando.Parameters.Add(paisParam);

            comando.Prepare();
            comando.ExecuteNonQuery();
            database.Desconectar();
        }

    }
}
