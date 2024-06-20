using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using U3ActRegistroDeActividadesMaui.Models.DTOs;

namespace U3ActRegistroDeActividadesMaui.Services
{
    public class DepartamentosService
    {
        private readonly HttpClient cliente;
        public DepartamentosService()
        {
            cliente = new()
            {
                BaseAddress = new Uri("https://u3eqpo1actapi.labsystec.net/api/Departamentos/")
            };
            ActualizarToken();
        }
        //Obtencion de token y validación de peticiones
        public async void ActualizarToken()
        {
            var token = await SecureStorage.GetAsync("tkn");
            if (string.IsNullOrEmpty(token))
            {
                throw new UnauthorizedAccessException("No autorizado");
            }
            cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
        //Cerrar sesion si la peticion http no es valida
        public async Task CerrarSesion()
        {
            await Shell.Current.DisplayAlert("Credenciales expiradas", "Inicia sesión nuevamente", "Aceptar");
            //eliminar token
            SecureStorage.Remove("tkn");
            //Enviar de regreso al login
            await Shell.Current.GoToAsync("/Login");
        }
        #region Read
        public async Task<DepartamentoDTO> GetDepartamentos(int id)
        {
            try
            {
                //Puedes usar esto como ejemplo para hacer peticiones a la API
                var response = await cliente.GetAsync($"Actividades/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<DepartamentoDTO>(json);
                    return result ?? new();
                }
            }
            catch (HttpRequestException excepción)
            {
                if (excepción.HttpRequestError == HttpRequestError.UserAuthenticationError)
                {
                    await CerrarSesion();
                }
            }
            catch (Exception e)
            {
                await Shell.Current.DisplayAlert("Error", $"Request error: {e.Message}", "Ok");
            }
            return new();
        }
        #endregion
        #region Create

        //public async Task Insert(DepartamentoDTO dto)
        //{
        //    try
        //    {
        //        var requestBody = new
        //        {
        //            id = dto.Id,
        //            nombre = dto.Departamento,
        //            username = dto.Username,
        //            password = dto.Password,
        //            idSuperior = dto.IdSuperior
        //        };

        //        var jsonContent = JsonConvert.SerializeObject(requestBody);
        //        Console.WriteLine($"Request JSON: {jsonContent}");

        //        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
        //        var response = await cliente.PostAsync("Agregar", content);

        //        if (!response.IsSuccessStatusCode)
        //        {
        //            var errorMessage = await response.Content.ReadAsStringAsync();
        //            Console.WriteLine($"Response Status: {response.StatusCode}");
        //            Console.WriteLine($"Response Error: {errorMessage}");
        //            await Shell.Current.DisplayAlert("Error", $"Error al enviar la solicitud: {response.StatusCode} - {errorMessage}", "Aceptar");
        //        }
        //    }
        //    catch (HttpRequestException ex)
        //    {
        //        await Shell.Current.DisplayAlert("Error", $"Error al enviar la solicitud: {ex.Message}", "Aceptar");
        //    }
        //    catch (Exception ex)
        //    {
        //        await Shell.Current.DisplayAlert("Error", ex.Message, "Aceptar");
        //    }
        //}

        //Este hace lo mismo que el de arriba pero es mas facil leerlo
        public async Task Insert(DepartamentoDTO dto)
        {
            try
            {
                var response = await cliente.PostAsJsonAsync("Agregar", dto);
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException excepción)
            {
                if (excepción.HttpRequestError == HttpRequestError.UserAuthenticationError)
                {
                    await CerrarSesion();
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "Aceptar");
            }
        }
        #endregion
        #region Update
        public async Task Update(DepartamentoDTO dto)
        {
            try
            {
                var response = await cliente.PutAsJsonAsync("Editar", dto);
                response.EnsureSuccessStatusCode();
            }

            catch (HttpRequestException excepción)
            {
                if (excepción.HttpRequestError == HttpRequestError.UserAuthenticationError)
                {
                    await CerrarSesion();
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "Aceptar");
            }
        }
        #endregion
        #region Delete
        public async Task Delete(DepartamentoDTO dto)
        {
            try
            {
                var response = await cliente.DeleteAsync($"Eliminar/{dto.Id}");
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException excepción)
            {
                if (excepción.HttpRequestError == HttpRequestError.UserAuthenticationError)
                {
                    await CerrarSesion();
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "Aceptar");
            }
        }
        #endregion
    }
}
