using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using U3ActRegistroDeActividadesMaui.Models.DTOs;
using U3ActRegistroDeActividadesMaui.Models.Entities;


namespace U3ActRegistroDeActividadesMaui.Services
{
    public class ActividadesService
    {
        private readonly HttpClient cliente;
        public ActividadesService()
        {
            cliente = new()
            {
                BaseAddress = new Uri("https://u3eqpo1actapi.labsystec.net/api/Actividades/")
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
        }
        #region Read
        public async Task<ActividadDTO> GetActividad(int id)
        {
            try
            {
                var response = await cliente.GetFromJsonAsync<ActividadDTO>($"{id}");
                if (response != null)
                {
                    return response;
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
        public async Task<IEnumerable<Actividades>?> GetAllRight()
        {
            try
            {
                if (cliente.DefaultRequestHeaders.Authorization == null)
                {
                    var token = await SecureStorage.GetAsync("tkn");
                    cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }
                var response = await cliente.GetAsync("GetAll");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var actividades = System.Text.Json.JsonSerializer.Deserialize<IEnumerable<Actividades>>(json, new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    return actividades;
                }
            }
            catch (HttpRequestException excepción)
            {
                if (excepción.HttpRequestError == HttpRequestError.UserAuthenticationError)
                {
                    await CerrarSesion();
                }
            }
            return null;
        }
        public async Task<IEnumerable<ActividadDTO>?> GetAll()
        {
            try
            {
                //Primero, crear un HttpRequestMessage
                //HttpRequestMessage rm = new();
                ////Se le da el tipo de peticion
                //rm.Method = HttpMethod.Get;
                ////Se le da el token
                //var token = await SecureStorage.GetAsync("tkn");
                //rm.Headers.Add("Authorization", $"Bearer {token}");

                //Si llega a tener contenido la peticion
                //var json = System.Text.Json.JsonSerializer.Serialize()
                //rm.Content = new StringContent("", Encoding.UTF8, "");
                var response = await cliente.GetAsync("GetAll");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var actividades = System.Text.Json.JsonSerializer.Deserialize<Actividades>(json);
                }
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
            return null;
        }
        public async Task<ActividadDTO?> Get(ActividadDTO dto)
        {
            try
            {
                var response = await cliente.GetAsync($"/Actividad/{dto.Id}");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<ActividadDTO>(json);
                    return result;
                }
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
            return null;
        }
        #endregion
        #region Create
        public async Task Insert(ActividadDTO dto)
        {
            try
            {
                var response = await cliente.PostAsJsonAsync($"Departamento/{dto.IdDepartamento}/Agregar", dto);
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
        public async Task Update(ActividadDTO dto)
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
        public async Task Delete(ActividadDTO dto)
        {
            try
            {
                ActualizarToken();
                var response = await cliente.DeleteAsync($"/Eliminar/{dto.Id}");
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
