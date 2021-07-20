using System;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace API_Tienda.Models
{
    //Clase con utilidades para el manejo de archivos.
    public class ImageUtilities
    {
        //Metodo para validar una imagen.
        public bool IsValidImage(string filename)
        {
            using (FileStream fs = File.OpenRead(filename))
            {
                byte[] header = new byte[10];
                fs.Read(header, 0, 10);

                foreach (var pattern in new byte[][] {
                    Encoding.ASCII.GetBytes("BM"),
                    Encoding.ASCII.GetBytes("GIF"),
                    new byte[] { 137, 80, 78, 71 },     // PNG
                    new byte[] { 73, 73, 42 },          // TIFF
                    new byte[] { 77, 77, 42 },          // TIFF
                    new byte[] { 255, 216, 255, 224 },  // jpeg
                    new byte[] { 255, 216, 255, 225 }   // jpeg Canon
                })
                if (pattern.SequenceEqual(header.Take(pattern.Length)))
                    return true;
            }
            return false;
        }

        //Metodo para redimensionar una imagen
        public void ResizeImage(string imagename, string path, float size)
        {
            Image old_image = Image.FromFile(Path.Combine(path, imagename));
            float factor = (float)Math.Sqrt(size / 1048576);
            factor = 1/factor;
            int new_width = (int)(old_image.Width * factor/3);
            int new_height = (int)(old_image.Height * factor/3);
            Bitmap new_image = new Bitmap(new_width, new_height);
            Graphics g = Graphics.FromImage((Image)new_image);
            g.DrawImage(old_image, 0, 0, new_width, new_height);
            new_image.Save(Path.Combine(path, "resized_" + imagename));
            new_image.Dispose();
            old_image.Dispose();
            g.Dispose();
        }
    }
}
