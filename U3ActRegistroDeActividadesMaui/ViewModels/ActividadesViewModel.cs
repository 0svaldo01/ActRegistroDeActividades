using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
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

        public ActividadesViewModel()
        {
            Actividades.Clear();
        }

        [ObservableProperty]
        private ActividadDTO? actividad;

        [ObservableProperty]
        private string error = "";

        [RelayCommand]
        public async void Nuevo()
        {
            Actividad = new();
            await Shell.Current.GoToAsync("//AgregarAct");
        }

        [RelayCommand]
        public async void Cancelar()
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
                        await service.Insert(Actividad);
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
