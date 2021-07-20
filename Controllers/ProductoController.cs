using API_Tienda.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace API_Tienda.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProductoController : Controller
    {
        [HttpGet("/api/v1/producto/search")]
        public ActionResult Get(string? nombre, decimal? preciomax, decimal? preciomin, byte? descuento, string? pais)
        {
            string query_string = "";
            if (!string.IsNullOrEmpty(nombre))
                query_string = query_string + " UPPER(nombre) like CONCAT('%', UPPER(@nombre), '%')";

            if (preciomax.HasValue && preciomin.HasValue && preciomax >= preciomin)
                query_string = query_string.Length > 0 ? query_string + " and precio <= @preciomax and precio >= @preciomin" : query_string + " precio <= @preciomax and precio >= @preciomin";
            
            if (descuento.HasValue)
                query_string = query_string.Length > 0 ? query_string + " and descuento = @descuento" : query_string + " descuento = @descuento";
            
            if (!string.IsNullOrEmpty(pais))
                query_string = query_string.Length > 0 ? query_string + " and UPPER(pais) like CONCAT('%', UPPER(@pais), '%')" : query_string + " UPPER(pais) like CONCAT('%', UPPER(@pais), '%')";
                        
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

        [HttpGet]
        public ActionResult GetAll()
        {
            Producto producto = new Producto();
            if (producto.GetAll().Count > 0)
                return Ok(producto.GetAll());
            else
                return NotFound();
        }

        [HttpPost("/api/v1/producto")]
        public ActionResult Post([FromForm] Producto data)
        {
            Producto producto = data;
            try
            {
                if (producto.pais.ToUpper() == "COLOMBIA" || producto.pais.ToUpper() == "MEXICO" && producto.descuento > 50)
                {
                    return BadRequest("The discount musn´t be greater than 50%");
                }
                else if (producto.pais.ToUpper() == "CHILE" || producto.pais.ToUpper() == "PERU" && producto.descuento > 30)
                {
                    return BadRequest("The discount musn´t be greater than 30%");
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
