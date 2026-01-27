using Microsoft.AspNetCore.Mvc;

namespace Apo.Service.AuthAPI.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class TestAuthController : ControllerBase
    {
        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok("Token is valid!");
        }
    }
}
