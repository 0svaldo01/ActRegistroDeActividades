using Microsoft.AspNetCore.Mvc;
using U3ActRegistroDeActividadesApi.Models.DTOs;
using U3ActRegistroDeActividadesApi.Models.Entities;
using U3ActRegistroDeActividadesApi.Models.Validators;
using U3ActRegistroDeActividadesApi.Repositories;

namespace U3ActRegistroDeActividadesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "Admin,Depto")]
    public class ActividadesController(ActividadesRepository Repositorio) : ControllerBase
    {
        #region CRUD
        #region Create
        [HttpPost("/Create")]
        public async Task<ActionResult<ActividadDTO>> PostActividad([FromForm] ActividadDTO dto)
        {
            ActividadDTOValidator validador = new();
            var result = validador.Validate(dto);
            if (result.IsValid)
            {

                var actividad = new Actividades
                {
                    Id = 0,
                    Titulo = dto.Titulo,
                    Descripcion = dto.Descripcion,
                    FechaRealizacion = dto.FechaRealizacion,
                    IdDepartamento = dto.IdDepartamento,
                    FechaCreacion = DateTime.UtcNow,
                    FechaActualizacion = DateTime.UtcNow,
                    Estado = dto.Estado,
                };
                Repositorio.Insert(actividad);
                //Primero se agrega el objeto para que la imagen pueda obtener el Id
                var filePath = Path.Combine($"wwwroot/Images/{actividad.Id}.jpg");

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await dto.Imagen.CopyToAsync(stream);
                }

                return CreatedAtAction(nameof(GetActividad), new { id = actividad.Id }, actividad);
            }
            return BadRequest("Ingrese los datos solicitados");
        }
        #endregion
        #region Read
        [HttpGet("/")]
        public IActionResult GetActividades()
        {
            var actividades = Repositorio.GetActividades().Select(actividad => new GetActividadDTO
            {
                Id = actividad.Id,
                Departamento = actividad.IdDepartamentoNavigation.Nombre,
                Descripcion = actividad.Descripcion,
                Estado = actividad.Estado,
                FechaActualizacion = actividad.FechaActualizacion,
                FechaCreacion = actividad.FechaCreacion,
                FechaRealizacion = actividad.FechaRealizacion,
                Titulo = actividad.Titulo
            });
            if (actividades.Any())
            {
                return Ok(actividades);
            }
            return NotFound("No hay actividades");
        }

        [HttpGet("/{id:int}")]
        public IActionResult GetActividad(int id)
        {
            var datos = Repositorio.GetActividad(id);
            if (datos != null)
            {
                GetActividadDTO actividad = new()
                {
                    Id = datos.Id,
                    Departamento = datos.IdDepartamentoNavigation.Nombre,
                    Descripcion = datos.Descripcion,
                    Estado = datos.Estado,
                    FechaActualizacion = datos.FechaActualizacion,
                    FechaCreacion = datos.FechaCreacion,
                    FechaRealizacion = datos.FechaRealizacion,
                    Titulo = datos.Titulo
                };
                return Ok(actividad);
            }
            return NotFound("No existe la actividad");
        }
        #endregion
        #region Update
        [HttpPut("/Update")]
        public async Task<IActionResult> EditarAsync([FromForm] ActividadDTO dto)
        {
            ActividadDTOValidator validador = new();
            var result = validador.Validate(dto);
            if (result.IsValid)
            {
                var actividad = Repositorio.GetActividad(dto.Id);
                if (actividad != null)
                {
                    if (actividad.IdDepartamento != dto.IdDepartamento)
                    {
                        return Conflict("La actividad pertenece a otro departamento");
                    }
                    actividad.Estado = dto.Estado > 0 ? dto.Estado : 0;
                    actividad.FechaActualizacion = DateTime.UtcNow; //fecha en la que se realizo el cambio
                    actividad.Descripcion = dto.Descripcion;
                    actividad.Titulo = dto.Titulo;
                    actividad.FechaRealizacion = dto.FechaRealizacion;
                    actividad.IdDepartamento = dto.IdDepartamento;
                    var filePath = Path.Combine($"wwwroot/Images/{dto.Id}.jpg");

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await dto.Imagen.CopyToAsync(stream);
                    }
                    Repositorio.Update(actividad);
                    return Ok("Actividad actualizada");
                }
                return NotFound("No existe la actividad que se desea eliminar");
            }
            return BadRequest("Selecciona la actividad que desee eliminar");
        }
        #endregion
        #region Delete
        [HttpDelete("/Delete/{id:int}")]
        public IActionResult Eliminar(int id)
        {
            var actividad = Repositorio.GetActividad(id);
            if (actividad != null && actividad.Estado != 2)
            {
                actividad.Estado = 2;    // Baja lógica
                actividad.FechaActualizacion = DateTime.UtcNow; //fecha en la que se realizo el cambio
                Repositorio.Update(actividad);
                return Ok("Actividad eliminada");
            }
            return NotFound("No existe la actividad que se desea eliminar");
        }
        #endregion
        #endregion
    }
}