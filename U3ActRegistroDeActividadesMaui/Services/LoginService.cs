using System.Text;
using System.Text.Json;
using U3ActRegistroDeActividadesMaui.Models.DTOs;

namespace U3ActRegistroDeActividadesMaui.Services
{
    public class LoginService
    {
        private readonly HttpClient cliente;
        public LoginService(HttpClient client)
        {
            cliente = client;
        }
        public bool Login(string username, string password)
        {
            var content = new StringContent(JsonSerializer.Serialize(
                new LoginDTO()
                {
                    correo = username,
                    contraseña = password
                }),
                Encoding.UTF8, "application/json");

            var response = cliente.PostAsync("api/login", content).Result;

            if (response.IsSuccessStatusCode)
            {
                SecureStorage.SetAsync("tkn", response.Content.ReadAsStringAsync().Result);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
