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
    public partial class DepartamentosViewModel : ObservableObject
    {
        DepartamentosRepository departamentosRepository = new();

        public ObservableCollection<Departamentos> Departamentos { get; set; } = new();
        DepartamentosService service = new();
        DepartamentoDTOValidator validator = new();

        public DepartamentosViewModel()
        {
            Departamentos.Clear();
            foreach (var dep in departamentosRepository.GetAll())
            {
                Departamentos.Add(dep);
            }
        }

        void ActualizarDepartamentos()
        {
            ActualizarDepartamentos();
            App.DepartamentosService.DatosActualizadosDep += DepartamentosService_DatosActualizadosDep;
        }

        private void DepartamentosService_DatosActualizadosDep()
        {
            ActualizarDepartamentos();
        }

        [ObservableProperty]
        private DepartamentoDTO? departamento;
        [ObservableProperty]
        private string error = "";


        [RelayCommand]
        public void Nuevo()
        {
            Departamento = new();
            Shell.Current.GoToAsync("//AgregarDep");
        }

        [RelayCommand]
        public void Cancelar()
        {
            Departamento = null;
            Error = "";
            Shell.Current.GoToAsync("//ListaDep");
        }

        [RelayCommand]
        public async Task Agregar()
        {
            try
            {
                if (Departamento != null)
                {
                    var resultado = validator.Validate(Departamento);
                    if (resultado.IsValid)
                    {
                        await service.Insert(Departamento);
                        ActualizarDepartamentos();
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
