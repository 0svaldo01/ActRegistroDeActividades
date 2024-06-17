using System.Security.Cryptography;
using System.Text;

namespace U3ActRegistroDeActividadesMaui.Helpers
{
    public class Encriptacion
    {
        public static string ConvertToSha512(string input)
        {
            byte[] data = Encoding.UTF8.GetBytes(input);
            byte[] conversion = SHA512.HashData(data);
            return Convert.ToHexString(conversion).ToLower();
        }
    }
}
