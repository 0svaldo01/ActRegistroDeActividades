using System.Security.Cryptography;
using System.Text;

namespace U3ActRegistroDeActividadesApi.Helpers
{
    public class Encriptacion
    {
        public static string EncriptarSHA512(string input)
        {
            byte[] temp = Encoding.UTF8.GetBytes(input);
            byte[] Hash = SHA512.HashData(temp);
            return Convert.ToHexString(Hash).ToLower();
        }
    }
}
