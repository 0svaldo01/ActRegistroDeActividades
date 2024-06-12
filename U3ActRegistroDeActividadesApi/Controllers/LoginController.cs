using Microsoft.AspNetCore.Mvc;
using U3ActRegistroDeActividadesApi.Helpers;
using U3ActRegistroDeActividadesApi.Models.DTOs;
using U3ActRegistroDeActividadesApi.Models.Security;
using U3ActRegistroDeActividadesApi.Models.Validators;
using U3ActRegistroDeActividadesApi.Repositories;

namespace U3ActRegistroDeActividadesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController(DepartamentosRepository Repositorio, IConfiguration config) : ControllerBase
    {
        [HttpPost]
        public IActionResult Login(LoginDTO dto)
        {
            LoginDTOValidator validador = new();
            var result = validador.Validate(dto);
            if (result.IsValid)
            {
                dto.Password = Encriptacion.EncriptarSHA512(dto.Password);
                var usuario = Repositorio.GetAll()
                    .FirstOrDefault(x => x.Username == dto.Username && x.Password == dto.Password);
                if (usuario != null)
                {
                    var jwt = config.GetSection("JWT").Get<JWT>();
                    if (jwt != null)
                    {
                        string token = JWTHelper.GetToken(usuario, jwt);
                        return Ok(token);
                    }
                    return StatusCode(500, "JWT no configurado");
                }
                return NotFound("Correo o contraseña incorrecta.");
            }
            return BadRequest(result.Errors);
        }
    }
}
