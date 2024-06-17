﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.IdentityModel.Tokens.Jwt;
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
        private readonly ActividadesRepository actividadesRepository = new();
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
            Iniciar();
        }
        #region Actualizar Listas
        private async void Iniciar()
        {
            var id = await GetToken();
            await HacerPeticionGet(id);
            //Id Token 97
            //IdSuperior 97 => 1
        }
        public async Task<int> GetToken()
        {
            var tkn = await SecureStorage.GetAsync("tkn");
            if (!string.IsNullOrEmpty(tkn))
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(tkn);
                var datos = jwtToken.Claims.ToDictionary(claim => claim.Type, claim => (object)claim.Value);
                var id = datos.FirstOrDefault(x => x.Key == "id").Value;
                var idDepto = int.Parse(id.ToString() ?? "");
                return idDepto;
            }
            return 0;
        }
        private async Task HacerPeticionGet(int id)
        {
            //Obtener los departamentos de la API
            var departamentosServer = await service.GetDepartamentos(id);
            if (departamentosServer.Id > 0)
            {
                if (departamentosServer.Subordinados.Any())
                {
                    departamentosRepository.DeleteAll();
                    //Agregar en local
                    foreach (var departamentoDTO in departamentosServer.Subordinados)
                    {
                        Departamentos entity = new()
                        {
                            Id = departamentoDTO.Id,
                            IdSuperior = departamentoDTO.IdSuperior,
                            Nombre = departamentoDTO.Departamento,
                            Username = departamentoDTO.Username,
                            Password = departamentoDTO.Password
                        };
                        if (!Departamentos.Contains(entity))
                        {
                            departamentosRepository.Insert(entity);
                            foreach (var actividad in departamentoDTO.Actividades)
                            {
                                Actividades act = new()
                                {
                                    Id = actividad.Id,
                                    Descripcion = actividad.Descripcion,
                                    Estado = actividad.Estado,
                                    FechaActualizacion = actividad.FechaActualizacion,
                                    FechaCreacion = actividad.FechaCreacion,
                                    FechaRealizacion = actividad.FechaRealizacion,
                                    IdDepartamento = actividad.IdDepartamento,
                                    Titulo = actividad.Titulo
                                };
                                actividadesRepository.Insert(act);
                            }
                        }
                    }
                    //Editar en local
                    foreach (var departamentoDTO in departamentosServer.Subordinados)
                    {
                        Departamentos entity = new()
                        {
                            Id = departamentoDTO.Id,
                            IdSuperior = departamentoDTO.IdSuperior,
                            Nombre = departamentoDTO.Departamento,
                            Username = departamentoDTO.Username,
                            Actividades = departamentoDTO.Actividades.Select(act => new Actividades
                            {
                                Id = act.Id,
                                Descripcion = act.Descripcion,
                                Estado = act.Estado,
                                FechaActualizacion = act.FechaActualizacion,
                                IdDepartamento = act.IdDepartamento,
                                FechaCreacion = act.FechaCreacion,
                                FechaRealizacion = act.FechaRealizacion,
                                Titulo = act.Titulo
                            }).ToList()
                        };
                        if (Departamentos.Contains(entity))
                        {
                            departamentosRepository.Update(entity);
                            foreach (var actividad in departamentoDTO.Actividades)
                            {
                                Actividades act = new()
                                {
                                    Id = actividad.Id,
                                    Descripcion = actividad.Descripcion,
                                    Estado = actividad.Estado,
                                    FechaActualizacion = actividad.FechaActualizacion,
                                    FechaCreacion = actividad.FechaCreacion,
                                    FechaRealizacion = actividad.FechaRealizacion,
                                    IdDepartamento = actividad.IdDepartamento,
                                    Titulo = actividad.Titulo,
                                };
                                actividadesRepository.Update(act);
                            }
                        }
                    }
                    //Eliminar en local
                    foreach (var departamentoDTO in departamentosServer.Subordinados)
                    {
                        if (Departamentos.FirstOrDefault(x => x.Id == departamentoDTO.Id) != null)
                        {

                            foreach (var actividad in departamentoDTO.Actividades)
                            {
                                Actividades act = new()
                                {
                                    Id = actividad.Id,
                                    Descripcion = actividad.Descripcion,
                                    Estado = actividad.Estado,
                                    FechaActualizacion = actividad.FechaActualizacion,
                                    FechaCreacion = actividad.FechaCreacion,
                                    FechaRealizacion = actividad.FechaRealizacion,
                                    IdDepartamento = actividad.IdDepartamento,
                                    Titulo = actividad.Titulo,
                                };
                                actividadesRepository.Delete(act);
                            }
                            departamentosRepository.Delete(departamentoDTO.Id);
                        }
                    }
                    //Actualizar lista
                    ActualizarDepartamentos();
                }
            }
        }
        void ActualizarDepartamentos()
        {
            Departamentos.Clear();
            //Traer la base de datos local
            foreach (var dep in departamentosRepository.GetAll())
            {
                Departamentos.Add(dep);
            }
        }
        #endregion
        #region Vistas
        [RelayCommand]
        public void Cancelar()
        {
            Departamento = new();
            //Error = "";
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