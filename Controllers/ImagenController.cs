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
        public ActionResult Upload([FromForm] string idproducto)
        {
            try
            {
                var files = Request.Form.Files;
                if (files.Any(f => f.Length == 0))
                {
                    return BadRequest();
                }
                string folderName = Path.Combine("Assets", "Images");
                string pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName, idproducto);
                if (!Directory.Exists(pathToSave))
                {
                    Directory.CreateDirectory(pathToSave);
                }
                string files_uploaded = "";
                int files_uploaded_count = 0;
                ImageUtilities imageUtilities = new ImageUtilities();
                foreach (var file in files)
                {
                    string fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    string fullPath = Path.Combine(pathToSave, fileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    if (imageUtilities.IsValidImage(fullPath)) {
                        if (file.Length > 1048576)
                        {
                            imageUtilities.resize(fileName, pathToSave, file.Length);
                            FileInfo file_to_delete = new FileInfo(fullPath);
                            file_to_delete.Delete();
                        }
                        files_uploaded_count++;
                        if (files_uploaded == "")
                            files_uploaded = fileName;
                        else
                            files_uploaded += "; " + fileName;
                    }
                    else
                    {                                               
                        FileInfo file_to_delete = new FileInfo(fullPath);
                        file_to_delete.Delete();
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
