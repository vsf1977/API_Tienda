using Microsoft.AspNetCore.Mvc;
using Restaurante_API.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Net;
using System.Net.Http;

namespace Restaurante_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClienteController : ControllerBase
    {
        [HttpGet]
        public List<Cliente> Get()
        {
            Cliente Cliente = new Cliente();
            return Cliente.list();
        }

        [HttpGet("/api/cliente/{id}")]
        public Cliente Get(string id)
        {
            Cliente Cliente = new Cliente();
            return Cliente.GetCliente(id);
        }
        [HttpPost("/api/cliente")]
        public HttpResponseMessage Post([FromBody] Cliente data)
        {
            Cliente Cliente = new Cliente();
            Cliente.IdCliente = data.IdCliente;
            Cliente.nombre = data.nombre;
            Cliente.apellidos = data.apellidos;
            Cliente.direccion = data.direccion;
            Cliente.telefono = data.telefono;
            try
            {
                Cliente.insertar();
                return new HttpResponseMessage(HttpStatusCode.Created);
            }
            catch (SqlException ex)
            {
                HttpResponseMessage resp = new HttpResponseMessage();
                resp.StatusCode = HttpStatusCode.InternalServerError;
                string v = ex.Message.Trim();
                int index = v.IndexOf("\r");
                v = v.Substring(0, index - 1);
                resp.ReasonPhrase = v;
                return resp;
            }
        }
        [HttpPut("/api/cliente")]
        public HttpResponseMessage Put([FromBody] Cliente data)
        {
            Cliente Cliente = new Cliente();
            Cliente.IdCliente = data.IdCliente;
            Cliente.nombre = data.nombre;
            Cliente.apellidos = data.apellidos;
            Cliente.direccion = data.direccion;
            Cliente.telefono = data.telefono;
            try
            {
                Cliente.actualizar();
                return new HttpResponseMessage(HttpStatusCode.Created);
            }
            catch (SqlException ex)
            {
                HttpResponseMessage resp = new HttpResponseMessage();
                resp.StatusCode = HttpStatusCode.InternalServerError;
                string v = ex.Message.Trim();
                int index = v.IndexOf("\r");
                v = v.Substring(0, index - 1);
                resp.ReasonPhrase = v;
                return resp;
            }
        }

        [HttpDelete("/api/cliente/{id}")]
        public HttpResponseMessage Delete(string id)
        {
            Cliente Cliente = new Cliente();
            Cliente.IdCliente = id;           
            try
            {
                Cliente.borrar();
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch (SqlException ex)
            {
                HttpResponseMessage resp = new HttpResponseMessage();
                resp.StatusCode = HttpStatusCode.InternalServerError;
                string v = ex.Message.Trim();
                int index = v.IndexOf("\r");
                v = v.Substring(0, index - 1);
                resp.ReasonPhrase = v;
                return resp;
            }
        }
        


    }
                
}
