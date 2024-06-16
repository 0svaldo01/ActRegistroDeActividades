﻿using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using U3ActRegistroDeActividadesMaui.Models.DTOs;

namespace U3ActRegistroDeActividadesMaui.Services
{
    public class DepartamentosService
    {
        private readonly HttpClient cliente;
        private readonly Repositories.DepartamentosRepository departamentosRepository = new();
        public DepartamentosService()
        {
            ////Este link puede cambiar
            //cliente = IPlatformApplication.Current != null ?
            //    IPlatformApplication.Current.Services.GetService<HttpClient>() ?? new HttpClient() : new();

            HttpClient Cliente;
            Cliente = new() 
            {
                BaseAddress = new Uri("https://u3eqpo1actapi.labsystec.net/")
            };
        }

        public event Action? DatosActualizadosDep;

        #region Read
        public async Task<string> GetTokenAsync()
        {
            return await SecureStorage.GetAsync("tkn");
        }
        
        public async Task<string> GetDepartamentosIntento()
        {
            try
            {
                string token = await GetTokenAsync();
                if (string.IsNullOrEmpty(token))
                {
                    throw new UnauthorizedAccessException("No autorizado");
                }
                cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await cliente.GetAsync("Departamentos");
                response.EnsureSuccessStatusCode();
                string responseData = await response.Content.ReadAsStringAsync();
                return responseData;

            }
            catch (HttpRequestException e)
            {              
                Console.WriteLine($"Request error: {e.Message}");
                return null;
            }
            catch (UnauthorizedAccessException e)
            {
                Console.WriteLine($"Authorization error: {e.Message}");
                return null;
            }

        }

        public async Task GetDepartamentos()
        {
            try
            {
                bool aviso = false;
                

              
                    var response = await cliente.GetFromJsonAsync<List<DepartamentoDTO>>($"/Departamentos");

                    if (response != null)
                    {
                        foreach (DepartamentoDTO departamento in response)
                        {
                            var anterior = departamentosRepository.Get(departamento.Id);

                            if (anterior == null)
                            {
                                anterior = new()
                                {
                                    Id = departamento.Id,
                                    Username = departamento.Username
                                };
                                departamentosRepository.Insert(anterior);
                                aviso = true;
                            }
                            else
                            {
                                anterior.Nombre = departamento.Nombre;

                            }

                            if (aviso)
                            {

                                _ = MainThread.InvokeOnMainThreadAsync(() =>
                                {
                                    DatosActualizadosDep?.Invoke();
                                });
                            }

                        }
                    }
                
            }
            catch
            {

            } 
        }

        public async Task<IEnumerable<DepartamentoDTO>?>? GetAll()
        {
            try
            {
                //Hacer la peticion a la api
                var response = await cliente.GetAsync("/Departamentos");
                if (response.IsSuccessStatusCode)
                {
                    //convertir el body en json
                    var json = await response.Content.ReadAsStringAsync();
                    //convertir el json en lista
                    var result = JsonConvert.DeserializeObject<IEnumerable<DepartamentoDTO>>(json);
                    //regresar lista de departamentos
                    return result;
                }
            }
            catch (Exception ex)
            {
                //Muestra posibles errores como mensaje en pantalla
                await Shell.Current.DisplayAlert("Error", ex.Message, "Aceptar");
            }
            return null;
        }
        public async Task<DepartamentoDTO?> Get(DepartamentoDTO dto)
        {
            try
            {
                var response = await cliente.GetAsync($"/Departamento/{dto.Id}");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<DepartamentoDTO>(json);
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
        public async Task Insert(DepartamentoDTO dto)
        {
            try
            {
                var response = await cliente.PostAsJsonAsync("/Agregar", dto);
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
                var response = await cliente.PutAsJsonAsync("/Editar", dto);
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
                var response = await cliente.DeleteAsync($"/Eliminar/{dto.Id}");
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
