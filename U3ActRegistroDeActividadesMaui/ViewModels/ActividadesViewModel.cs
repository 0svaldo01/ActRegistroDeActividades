using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
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
        private ActividadDTO? actividad;

        [ObservableProperty]
        private string error = "";

        [RelayCommand]
        public void VerListaDeDepartamentos()
        {
            Error = "";
            Shell.Current.GoToAsync("//ListaAct");
        }
        [RelayCommand]
        public void VerAgregarActividad()
        {
            Actividad = new();
            Error = "";
            Shell.Current.GoToAsync("//AgregarAct");
        }
        [RelayCommand]
        public async Task Cancelar()
        {
            Actividad = null;
            Error = "";
            await Shell.Current.GoToAsync("//ListaAct");
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
                        Actividad.Estado = 1;
                        //si existe
                        if (anterior != null)
                        {
                            //se edita
                            await service.Update(Actividad);
                        }
                        else
                        {
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
                        //Editar si existe
                        if (anterior != null && anterior.Estado != 2)//Eliminado
                        {
                            Actividad.Estado = 0; //Borrador
                            await service.Update(Actividad);
                        }
                        //Agregar si no existe
                        else
                        {
                            Actividad.Estado = 0; //Borrador
                            await service.Insert(Actividad);
                        }
                        //Regresar a la vista anterior
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
    }
}