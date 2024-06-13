using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace U3ActRegistroDeActividadesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Administrador,Departamento")]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public IActionResult test()
        {
            return Ok();
        }
    }
}
