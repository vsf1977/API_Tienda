using Microsoft.AspNetCore.Mvc;

namespace API_Tienda.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProductoController : Controller
    {
        [HttpGet]
        public int Get()
        {
            return 1;
        }
    }
}
