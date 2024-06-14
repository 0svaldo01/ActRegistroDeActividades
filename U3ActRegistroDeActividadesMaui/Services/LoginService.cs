using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using U3ActRegistroDeActividadesMaui.Models.DTOs;

namespace U3ActRegistroDeActividadesMaui.Services
{
    public class LoginService
    {
        HttpClient cliente;
        public LoginService(HttpClient cliente)
        {
            this.cliente = cliente;
            
        }
        public bool Login(string username, string password)
        {
            var content = new StringContent(JsonSerializer.Serialize(new LoginDTO() 
            {
                Username = username,
                Password = password
            }), Encoding.UTF8, "application/json");
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
