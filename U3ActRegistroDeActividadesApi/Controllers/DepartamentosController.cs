using Microsoft.AspNetCore.Mvc;
using U3ActRegistroDeActividadesApi.Helpers;
using U3ActRegistroDeActividadesApi.Models.DTOs;
using U3ActRegistroDeActividadesApi.Models.Entities;
using U3ActRegistroDeActividadesApi.Models.Validators;
using U3ActRegistroDeActividadesApi.Repositories;

namespace U3ActRegistroDeActividadesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "Admin,Depto")]
    public class DepartamentosController(DepartamentosRepository departamentosRepository) : ControllerBase
    {
        [HttpGet("/Actividades/{id:int}")]
        public IActionResult GetDepartamentoActividades(int id)
        {
            var depto = departamentosRepository.GetActividadesRecursivasPorDepartamento(id);
            return depto != null ? Ok(depto) : NotFound("No hay departamentos");
        }

        [HttpPost("/Agregar")]
        public IActionResult Agregar(DepartamentoDTO dto)
        {
            DepartamentoDTOValidator validador = new();
            var result = validador.Validate(dto);
            if (result.IsValid)
            {
                Departamentos depto = new()
                {
                    Id = 0,
                    IdSuperior = dto.IdSuperior > 0 ? dto.IdSuperior : null,
                    Nombre = dto.Nombre,
                    Password = Encriptacion.EncriptarSHA512(dto.Password),
                    Username = dto.Username
                };


                departamentosRepository.Insert(depto);
                return Ok("Se ah agregado el departamento exitosamente");
            }
            return BadRequest("Ingrese los datos solicitados");
        }

        [HttpPut("/Editar")]
        public IActionResult Editar(DepartamentoDTO dto)
        {
            DepartamentoDTOValidator validador = new();
            var result = validador.Validate(dto);
            if (result.IsValid)
            {
                var depto = departamentosRepository.GetDepartamento(dto.Id);
                if (depto != null)
                {
                    depto.Id = dto.Id;
                    depto.Nombre = dto.Nombre;
                    depto.Username = dto.Username;
                    depto.IdSuperior = dto.IdSuperior;
                    depto.Password = Encriptacion.EncriptarSHA512(dto.Password);
                    departamentosRepository.Update(depto);
                    return Ok("Se ah agregado el departamento exitosamente");
                }
                return NotFound("No se ha encontrado el departamento que desea actualizar");
            }
            return BadRequest("Ingrese los datos solicitados");
        }

        [HttpDelete("/Eliminar/{id:int}")]
        public IActionResult Eliminar(int id)
        {
            var departamento = departamentosRepository.GetDepartamento(id);
            if (departamento != null)
            {
                departamentosRepository.EliminarDepartamento(departamento);
                return Ok("Se ah eliminado el departamento");
            }
            return NotFound("No se ha encontrado el departamento");
        }
    }
}