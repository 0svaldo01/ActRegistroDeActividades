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
        public async Task<bool> Login(string username, string password)
        {
            var content = new StringContent(JsonSerializer.Serialize(
                new LoginDTO()
                {
                    correo = username,
                    contraseña = password
                }),
                Encoding.UTF8, "application/json");

            var response = await cliente.PostAsync("api/login", content);

            if (response.IsSuccessStatusCode)
            {
                await SecureStorage.SetAsync("tkn", await response.Content.ReadAsStringAsync());
                return true;
            }
            return false;
        }
    }
}
