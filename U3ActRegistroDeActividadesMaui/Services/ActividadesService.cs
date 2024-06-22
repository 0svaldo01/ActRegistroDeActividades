using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using U3ActRegistroDeActividadesMaui.Models.DTOs;


namespace U3ActRegistroDeActividadesMaui.Services
{
    public class ActividadesService
    {
        private readonly HttpClient cliente;
        public ActividadesService()
        {
            cliente = new()
            {
                BaseAddress = new Uri("http://u3eqpo1actapi.com/api/Actividades/")
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
            catch (HttpRequestException)
            {
                await CerrarSesion();
            }
            catch (Exception e)
            {
                await Shell.Current.DisplayAlert("Error", $"Request error: {e.Message}", "Ok");
            }
            return new();
        }
        public async Task<IEnumerable<ActividadDTO>?>? GetAll()
        {
            try
            {
                var response = await cliente.GetAsync("/Actividades");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<IEnumerable<ActividadDTO>>(json);
                    return result;
                }
            }
            catch (HttpRequestException)
            {
                await CerrarSesion();
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
            var tokenobtenido = SecureStorage.GetAsync("tkn");
            var handler = new JwtSecurityTokenHandler();

            //var jsonToken = handler.ReadToken(tokenobtenido) as JwtSecurityToken;
            //if (tokenobtenido != null)
            //{
                //var idClaim = jsonToken.Claims.FirstOrDefault(claim => claim.Type == "id");
            //}

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
