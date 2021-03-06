using API_Tienda.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data.SqlClient;

namespace API_Tienda.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]

    //Clase para el manejo de productos.
    public class ProductoController : Controller
    {

        //Metodo para hacer consultas basadas en uno o varios parametros.
        [HttpGet("/api/v1/producto/search")]
        public ActionResult Get(string? nombre, decimal? preciomax, decimal? preciomin, byte? descuento, string? pais)
        {
            //Dependiendo de que parametros este en la consulta, se va construyendo el query string.
            string query_string = "";
            if (!string.IsNullOrEmpty(nombre))
                query_string = query_string + " UPPER(nombre) like CONCAT('%', UPPER(@nombre), '%')";

            if (preciomax.HasValue && preciomin.HasValue && preciomax >= preciomin)
                query_string = query_string.Length > 0 ? query_string + " and precio <= @preciomax and precio >= @preciomin" : query_string + " precio <= @preciomax and precio >= @preciomin";

            if (descuento.HasValue)
                query_string = query_string.Length > 0 ? query_string + " and descuento = @descuento" : query_string + " descuento = @descuento";

            if (!string.IsNullOrEmpty(pais))
                query_string = query_string.Length > 0 ? query_string + " and UPPER(pais) like CONCAT('%', UPPER(@pais), '%')" : query_string + " UPPER(pais) like CONCAT('%', UPPER(@pais), '%')";

            if ((preciomax.HasValue ^ preciomin.HasValue) && !(preciomax.HasValue && preciomin.HasValue)  || preciomin > preciomax)
                return BadRequest("Parameter Error: The max price and the the min price must be valid and the max price must be greater than the min price");

            //Si el query string tiene algun parametro se hace la consulta
            if (query_string.Length > 0)
            {
                query_string = "select * from producto where" + query_string;
                try
                {
                    Producto producto = new Producto();
                    return Ok(producto.list(nombre, preciomax, preciomin, descuento, pais, query_string));
                }
                catch (SqlException ex)
                {
                    return StatusCode(500, $"Internal server error: {ex.Message}");
                }
            }
            else
            {
                return BadRequest("Parameter Error");
            }
        }

        //Metodo para traer todos los productos
        [HttpGet]
        public ActionResult GetAll()
        {
            Producto producto = new Producto();
            if (producto.GetAll().Count > 0)
                return Ok(producto.GetAll());
            else
                return NotFound();
        }

        //Metodo para guardar un producto nuevo
        [HttpPost("/api/v1/producto")]
        public ActionResult Post([FromForm] Producto data)
        {
            Producto producto = data;
            try
            {
                //Se verifica el descuento dependiendo del pais
                if ((producto.pais.ToUpper() == "COLOMBIA" && producto.descuento > 50) || (producto.pais.ToUpper() == "MEXICO" && producto.descuento > 50))
                {
                    return BadRequest("The discount musn´t be greater than 50% in colombia and mexico");
                }
                else if ((producto.pais.ToUpper() == "CHILE" && producto.descuento > 30) || (producto.pais.ToUpper() == "PERU" && producto.descuento > 30))
                {
                    return BadRequest("The discount musn´t be greater than 30% in chile and peru");
                }
                else
                {
                    producto.insertar();
                    return Created("api/v1/producto", data);
                }
            }
            catch (SqlException ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}
