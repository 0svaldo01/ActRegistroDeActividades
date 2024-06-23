namespace PerfectLoginApi.Helpers
{
    public class ConvertirImagen
    {
        public static string ObtenerImagen(int id)
        {
            //Obtiene la ruta de la imagen
            string imagePath = Path.GetFullPath($"wwwroot/Images/{id}.jpg");
            // Crear una instancia de FileInfo para obtener información del archivo
            FileInfo fileInfo = new(imagePath);
            //Verificar si la imagen no existe
            if (!fileInfo.Exists)
            {
                //Asigna una por defecto en caso de que no exista
                imagePath = Path.Combine($"wwwroot/Images/nophoto.jpg");
            }
            //Convierte la imagen en un arreglo de bytes
            byte[] bytes = File.ReadAllBytes(imagePath);
            //Convierte la imagen a base64
            imagePath = Convert.ToBase64String(bytes);
            //Regresa la imagen en base64
            return imagePath;
        }
        public static async Task GuardarImagenAsync(string nombreArchivo, string imagenBase64)
        {
            //Convierte la imagen en base64 a un arreglo de bytes
            byte[] imagen = Convert.FromBase64String(imagenBase64);
            //Ruta donde se guardara la imagen
            string filePath = Path.Combine("wwwroot/Images", nombreArchivo); //.jpg
            //Para crear el archivo
            using var stream = new FileStream(filePath, FileMode.Create);
            //Creacion de archivo
            await stream.WriteAsync(imagen);
        }

        public static string ConvertirABase64(string path)
        {
            byte[] bytes = File.ReadAllBytes(path);

            return Convert.ToBase64String(bytes);
        }
    }
}
