using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PerfectLoginApi.Helpers;
using System.Collections.ObjectModel;
using System.IdentityModel.Tokens.Jwt;
using U3ActRegistroDeActividadesMaui.Helpers;
using U3ActRegistroDeActividadesMaui.Models.DTOs;
using U3ActRegistroDeActividadesMaui.Models.Entities;
using U3ActRegistroDeActividadesMaui.Models.Validators;
using U3ActRegistroDeActividadesMaui.Services;

namespace U3ActRegistroDeActividadesMaui.ViewModels
{
    public partial class ActividadesViewModel : ObservableObject
    {
        public ObservableCollection<Actividades> Actividades { get; set; } = [];
        private readonly ActividadesService service = new();
        private readonly ActividadDTOValidator validador = new();
        private readonly ActividadesService actividadesService;
        public string Imagen = "";
        public ActividadesViewModel()
        {
            var service = IPlatformApplication.Current.Services.GetService<ActividadesService>() ?? new();
            this.actividadesService = service;

            Actividades.Clear();
            if (Connectivity.Current.NetworkAccess == NetworkAccess.Internet)
            {
                _ = CargarActividades();
            }
            else
            {
                //Cargar actividades de serializacion
                var acts = ActividadesSerializerHelper.Deserializar();
                foreach (var item in acts)
                {
                    Actividades.Add(item);
                }
            }
        }
        async Task CargarActividades()
        {
            Actividades.Clear();
            var acts = await actividadesService.GetAllRight();
            if (acts != null)
            {
                foreach (var item in acts)
                {
                    Actividades.Add(item);
                }
                //Serializar aqui
                ActividadesSerializerHelper.Serializar(Actividades);
            }
            await Task.CompletedTask;
        }
        [ObservableProperty]
        private ActividadDTO? actividad = new();
        [ObservableProperty]
        private string error = "";
        [RelayCommand]
        public async Task VerListaDeDepartamentos()
        {
            Error = "";
            await Shell.Current.GoToAsync("//ListaDep");
        }
        [RelayCommand]
        public void VerAgregarActividad()
        {
            Actividad = new();
            Error = "";
            Shell.Current.GoToAsync("//AgregarAct");
        }
        public async Task<Dictionary<string, object>> GetToken()
        {
            var tkn = await SecureStorage.GetAsync("tkn");
            if (!string.IsNullOrEmpty(tkn))
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(tkn);
                var datos = jwtToken.Claims.ToDictionary(claim => claim.Type, claim => (object)claim.Value);
                //var id = datos.FirstOrDefault(x => x.Key == "id").Value;
                //var idDepto = int.Parse(id.ToString() ?? "");
                return datos;
            }
            return null!;
        }
        [RelayCommand]
        public async Task Cancelar()
        {
            Error = "";
            await Shell.Current.GoToAsync("//ListaAct");
            await CargarActividades();
        }
        [RelayCommand]
        public async Task Agregar()
        {
            try
            {
                if (Actividad != null)
                {
                    var resultado = validador.Validate(Actividad);
                    if (resultado.IsValid)
                    {
                        //buscar actividad anterior
                        var anterior = await service.GetActividad(Actividad.Id);
                        Actividad.Estado = 1; //Publicado
                        //buscar el token
                        var token = await GetToken();
                        //obtener id
                        var idtoken = token.FirstOrDefault(x => x.Key == "id").Value;
                        //convertir a int
                        int id = int.Parse(idtoken.ToString() ?? "0");
                        //Asignar id
                        Actividad.IdDepartamento = id;
                        Actividad.FechaActualizacion = DateTime.UtcNow;

                        //si existe
                        if (anterior.Id != 0)
                        {
                            //se edita
                            await service.Update(Actividad);
                        }
                        else
                        {
                            Actividad.Id = 0;
                            Actividad.FechaCreacion = DateTime.UtcNow;
                            //se agrega si no existe
                            await service.Insert(Actividad);
                        }
                        //regresa a la vista de actividades
                        await Cancelar();
                    }
                    else
                    {
                        Error = string.Join("\n", resultado.Errors.Select(x => x.ErrorMessage));
                    }
                }
            }
            catch (Exception ex)
            {
                Error = ex.Message;
            }
        }
        [RelayCommand]
        public async Task Editar()
        {
            try
            {
                if (Actividad != null)
                {
                    var resultado = validador.Validate(Actividad);
                    if (resultado.IsValid)
                    {
                        Actividad.Estado = 1; //Publicado
                        await service.Update(Actividad);
                        await Cancelar();
                    }
                    else
                    {
                        Error = string.Join("\n", resultado.Errors.Select(x => x.ErrorMessage));
                    }
                }
            }
            catch (Exception ex)
            {
                Error = ex.Message;
            }
        }
        //Al presionar el boton de cancelar se llamara este metodo
        [RelayCommand]
        public async Task Guardar()
        {
            try
            {
                //Si la actividad no es nula
                if (Actividad != null)
                {
                    //y es valida
                    var resultado = validador.Validate(Actividad);
                    if (resultado.IsValid)
                    {
                        //Obtendrá la version anterior de la actividad
                        var anterior = await service.Get(Actividad);

                        Actividad.Estado = 0; //Borrador
                        //Editar si existe
                        if (anterior != null && anterior.Estado != 2)//Eliminado
                        {
                            await service.Update(Actividad);
                        }
                        //Agregar si no existe
                        else
                        {
                            await service.Insert(Actividad);
                        }
                        //Regresar a la vista anterior
                        await Cancelar();
                    }
                    else if (string.IsNullOrWhiteSpace(Actividad.Titulo)
                        && string.IsNullOrWhiteSpace(Actividad.Descripcion)
                        && string.IsNullOrWhiteSpace(Actividad.Imagen))
                    {
                        await Cancelar();
                    }
                    else
                    {
                        Error = string.Join("\n", resultado.Errors.Select(x => x.ErrorMessage));
                    }
                }
            }
            catch (Exception ex)
            {
                Error = ex.Message;
            }
        }
        //bool VerificarConexion()
        //{
        //    var conexion = Connectivity.NetworkAccess;
        //    return conexion == NetworkAccess.Internet;
        //}
        [RelayCommand]
        public async Task BuscarImagen()
        {
            try
            {
                var result = await FilePicker.PickAsync(new PickOptions
                {
                    FileTypes = FilePickerFileType.Images,
                    PickerTitle = "Selecciona una imagen"
                });
                if (result != null)
                {

                    FileInfo file = new(result.FullPath);
                    if (file.Exists)
                    {
                        Imagen = result.FullPath;

                        OnPropertyChanged(nameof(Imagen));
                        string Base64 = ConvertirImagen.ConvertirABase64(result.FullPath);
                        if (Actividad != null)
                        {
                            //Asignar imagen a la actividad
                            Actividad.Imagen = Base64;
                            //Actualizar
                            OnPropertyChanged(nameof(Actividad));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejar excepciones según sea necesario
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
            await Task.CompletedTask;
        }
    }
}