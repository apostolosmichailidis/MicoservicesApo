using Apo.Service.PaymentAPI_v2.Models;
using Microsoft.AspNetCore.Mvc;

namespace Apo.Service.PaymentAPI_V2.Controllers
{
    public class ApiControllerBase : ControllerBase
    {
        protected IActionResult ApiResponse(object? result, bool success = true, string? message = null)
        {
            return Ok(new ResponseDto
            {
                Result = result,
                IsSuccess = success,
                Message = message
            });
        }
    }
}
