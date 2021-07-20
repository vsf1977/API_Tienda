using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using API_Tienda.Models;

namespace API_Tienda.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ImagenController : Controller
    {
        [HttpPost("/api/v1/imagen")]
        //public ActionResult Upload()
        //Metodo para subir imagenes, recibiendo varios archivos y el id de producto.
        public ActionResult Upload([FromForm] string idproducto)
        {
            try
            {
                var files = Request.Form.Files;
                string folderName = Path.Combine("Assets", "Images"); 
                string pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName, idproducto);
                if (!Directory.Exists(pathToSave))
                {
                    //Se crea carpeta con el idproducto, donde se guardaran los archivos.
                    Directory.CreateDirectory(pathToSave);
                }
                string files_uploaded = "";
                ImageUtilities imageUtilities = new ImageUtilities();
                foreach (var file in files)
                {
                    //Se hace la revision y carga del archivo si tiene mas de 0 bytes.
                    if (file.Length > 0)
                    {
                        string fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                        string fullPath = Path.Combine(pathToSave, fileName);
                        using (var stream = new FileStream(fullPath, FileMode.Create))
                        {
                            //Se guarda el archivo
                            file.CopyTo(stream);
                        }
                        //Se valida que sea una imagen. Esto se debe hacer con el archivo salvado en el servidor.
                        if (imageUtilities.IsValidImage(fullPath))
                        {
                            //Si el archivo pesa mas de un mega, se redimensiona y se borra el original.
                            if (file.Length > 1048576)
                            {
                                imageUtilities.ResizeImage(fileName, pathToSave, file.Length);
                                FileInfo file_to_delete = new FileInfo(fullPath);
                                file_to_delete.Delete();
                            }
                            //Se carga el nombre del archivo cargado
                            if (files_uploaded == "")
                                files_uploaded = fileName;
                            else
                                files_uploaded += "; " + fileName;
                        }
                        else
                        {
                            //Si no es imagen, se borra.
                            FileInfo file_to_delete = new FileInfo(fullPath);
                            file_to_delete.Delete();
                        }
                    }
                }
                return Ok("The following files were loaded : " + files_uploaded);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

    }

}
