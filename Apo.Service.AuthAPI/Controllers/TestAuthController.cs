using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Apo.Service.AuthAPI.Controllers
{
    [ApiController]
    [Route("api/test")]
    public class TestAuthController : ControllerBase
    {
        [Authorize("Admin")]
        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok("Token is valid!");
        }
    }
}
