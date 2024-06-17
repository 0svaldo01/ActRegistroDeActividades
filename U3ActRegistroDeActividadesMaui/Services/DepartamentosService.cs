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
        public async void ActualizarToken()
        {
            var token = await SecureStorage.GetAsync("tkn");
            if (string.IsNullOrEmpty(token))
            {
                throw new UnauthorizedAccessException("No autorizado");
            }
            cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
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
            catch (Exception e)
            {
                await Shell.Current.DisplayAlert("Error", $"Request error: {e.Message}", "Ok");
            }
            return new();
        }
        #endregion
        #region Create
        public async Task Insert(DepartamentoDTO dto)
        {
            try
            {
                var response = await cliente.PostAsJsonAsync("Agregar", dto);
                response.EnsureSuccessStatusCode();
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
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "Aceptar");
            }
        }
        #endregion
    }
}
