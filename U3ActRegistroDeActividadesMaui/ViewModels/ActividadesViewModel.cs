using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using U3ActRegistroDeActividadesMaui.Models.DTOs;
using U3ActRegistroDeActividadesMaui.Models.Entities;
using U3ActRegistroDeActividadesMaui.Models.Validators;
using U3ActRegistroDeActividadesMaui.Repositories;
using U3ActRegistroDeActividadesMaui.Services;

namespace U3ActRegistroDeActividadesMaui.ViewModels
{
    public partial class ActividadesViewModel : ObservableObject
    {
        ActividadesRepository actividadesRepository = new();
        public ObservableCollection<Actividades> Actividades { get; set; } = new();
        ActividadesService service = new();
        ActividadDTOValidator validador = new();

        public ActividadesViewModel()
        {
            Actividades.Clear();
            foreach (var act in actividadesRepository.GetAll())
            {
                Actividades.Add(act);
            }
        }

        void ActualizarActividades()
        {
            ActualizarActividades();
            App.ActividadesService.DatosActualizadosAct += ActividadesService_DatosActualizados;
        }

        private void ActividadesService_DatosActualizados()
        {
            ActualizarActividades();
        }

        [ObservableProperty]
        private ActividadDTO? actividad;

        [ObservableProperty]
        private string error = "";

        [RelayCommand]
        public void Nuevo()
        {
            Actividad = new();
            Shell.Current.GoToAsync("//AgregarAct");
        }

        [RelayCommand]
        public void Cancelar()
        {
            Actividad = null;
            Error = "";
            Shell.Current.GoToAsync("//ListaAct");
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
                        await service.Insert(Actividad);
                        ActualizarActividades();
                        Cancelar();
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
    }
}
