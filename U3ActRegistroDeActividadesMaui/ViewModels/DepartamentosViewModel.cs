using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using U3ActRegistroDeActividadesMaui.Models.DTOs;
using U3ActRegistroDeActividadesMaui.Models.Entities;
using U3ActRegistroDeActividadesMaui.Models.Validators;
using U3ActRegistroDeActividadesMaui.Repositories;
using U3ActRegistroDeActividadesMaui.Services;

namespace U3ActRegistroDeActividadesMaui.ViewModels
{
    public partial class DepartamentosViewModel : ObservableObject
    {
        #region Repositorios
        private readonly DepartamentosRepository departamentosRepository = new();
        #endregion

        #region Listas
        public ObservableCollection<Departamentos> Departamentos { get; set; } = [];
        #endregion
        #region Modelos
        public Departamentos DepartamentoSeleccionado { get; set; } = new();

        [ObservableProperty]
        private DepartamentoDTO departamento = new();
        [ObservableProperty]
        private string error = "";
        #endregion
        #region Servicios
        private readonly DepartamentosService service = new();
        #endregion
        #region Validadores
        private readonly DepartamentoDTOValidator validator = new();
        #endregion
        public DepartamentosViewModel()
        {
            #region Subscripciones
            App.DepartamentosService.DatosActualizadosDep += DepartamentosService_DatosActualizadosDep;
            #endregion
        }
        #region Actualizar Listas
        void ActualizarDepartamentos()
        {
            Departamentos.Clear();
            //Traer la base de datos local
            foreach (var dep in departamentosRepository.GetAll())
            {
                Departamentos.Add(dep);
            }
            DepartamentosService_DatosActualizadosDep();
        }

        private void DepartamentosService_DatosActualizadosDep()
        {
            //Espera 1 segundos
            Task.Delay(1000);
            ActualizarDepartamentos();
        }
        #endregion

        #region Vistas
        [RelayCommand]
        public void Cancelar()
        {
            Departamento = new();
            Error = "";
            Shell.Current.GoToAsync("//ListaDep");
        }

        [RelayCommand]
        public async Task VerAgregarDepartamento()
        {
            await Shell.Current.GoToAsync("//AgregarDepView");
        }
        #endregion
        #region Comandos
        #region Create
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
        #endregion
        #region Read
        //Hacer una peticion Get a la api
        #endregion
        #region Update
        //Hacer una peticion Put a la api
        #endregion
        #region Delete
        //Hacer una peticion Delete a la api
        #endregion
        #endregion
    }
}
