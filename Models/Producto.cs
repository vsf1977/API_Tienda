using API_Tienda.DataAccess;
using System.Data.SqlClient;

namespace API_Tienda.Models
{
    public class Producto
    {
        SqlCommand comando;
        SqlDataReader cursor;
        Database database = new Database();
        public int idProducto { get; set; }
        public string nombre { get; set; }
        public decimal precio { get; set; }
        public byte descuento { get; set; }
        public string pais { get; set; }

    }
}
