using Newtonsoft.Json;
using System.Net.Http.Json;
using U3ActRegistroDeActividadesMaui.Models.DTOs;
using U3ActRegistroDeActividadesMaui.Repositories;

namespace U3ActRegistroDeActividadesMaui.Services
{
    public class ActividadesService
    {
        private readonly ActividadesRepository actividadesRepository = new();
        private readonly HttpClient cliente;
        public ActividadesService()
        {
            cliente = IPlatformApplication.Current != null ?
                IPlatformApplication.Current.Services.GetService<HttpClient>() ?? new HttpClient() : new();
            cliente.BaseAddress = new Uri("http://u3eqpo1actapi.com/api/");
        }

        public event Action? DatosActualizadosAct;
        #region Read
        public async Task GetActividades(int id)
        {
            try
            {
                var fecha = Preferences.Get("UltimaFechaActualizacion", DateTime.MinValue);
                bool aviso = false;

                var response = await cliente.GetFromJsonAsync<List<ActividadDTO>>($"/Actividades/{id}");
                if (response != null)
                {
                    foreach (ActividadDTO actividad in response)
                    {
                        var entidad = actividadesRepository.Get(actividad.Id);

                        //estado 0 = Borrador, estado 1 = Publicado, estado 2 = eliminado
                        if (entidad != null && (actividad.Estado == 0 || actividad.Estado == 1)
                            && actividad.FechaRealizacion != null) // 2 si esta eliminado
                        {
                            entidad = new()
                            {
                                Id = actividad.Id,
                                Titulo = actividad.Titulo,
                                Descripcion = actividad.Descripcion,
                                FechaRealizacion = new DateTime(actividad.FechaRealizacion.Value.Year, actividad.FechaRealizacion.Value.Month, actividad.FechaRealizacion.Value.Day),
                            };
                            actividadesRepository.Insert(entidad);
                            aviso = true;
                        }
                        else
                        {
                            if (entidad != null)
                            {
                                if (actividad.Estado == 2)
                                {
                                    actividadesRepository.Delete(entidad);
                                    aviso = true;
                                }
                                else
                                {
                                    if (actividad.Titulo != entidad.Titulo || actividad.Descripcion != actividad.Descripcion
                                        || actividad.FechaRealizacion != actividad.FechaRealizacion)
                                    {
                                        actividadesRepository.Update(entidad);
                                        aviso = true;
                                    }
                                }
                            }
                        }
                    }
                    if (aviso)
                    {

                        _ = MainThread.InvokeOnMainThreadAsync(() =>
                        {
                            DatosActualizadosAct?.Invoke();
                        });
                    }
                    Preferences.Set("UltimaFechaActualizacion", response.Max(x => x.FechaActualizacion));
                }
            }
            catch { }
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
                var response = await cliente.PostAsJsonAsync("/AgregarAct", dto);
                response.EnsureSuccessStatusCode();
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
                var response = await cliente.PutAsJsonAsync("/EditarAct", dto);
                response.EnsureSuccessStatusCode();
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
                var response = await cliente.DeleteAsync($"/EliminarAct/{dto.Id}");
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
