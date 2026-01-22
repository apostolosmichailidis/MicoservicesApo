using Apo.Service.AuthAPI.Helpers;
using Apo.Service.AuthAPI.Models.Dto;
using Apo.Service.AuthAPI.Service.IService;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Apo.Service.AuthAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthAPIController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthAPIController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<ResponseDTO>> Register(
            [FromServices] IValidator<RegistrationRequestDTO> validator,
            [FromBody] RegistrationRequestDTO model)
        {
            var validationResult = await ValidationHelper.ValidateAsync(model, validator);
            if (validationResult != null)
                return validationResult;

            var errorMessage = await _authService.Register(model);

            return string.IsNullOrEmpty(errorMessage)
                ? Ok(new ResponseDTO())
                : BadRequest(new ResponseDTO { IsSuccess = false, Message = errorMessage });
        }

        [HttpPost("login")]
        public async Task<ActionResult<ResponseDTO>> Login(
            [FromServices] IValidator<LoginRequestDTO> validator,
            [FromBody] LoginRequestDTO model)
        {
            var validationResult = await ValidationHelper.ValidateAsync(model, validator);
            if (validationResult != null)
                return validationResult;

            var loginResponse = await _authService.Login(model);

            if (loginResponse?.User == null)
            {
                return BadRequest(new ResponseDTO
                {
                    IsSuccess = false,
                    Message = "Invalid username or password."
                });
            }

            return Ok(new ResponseDTO { Result = loginResponse });
        }

        [HttpPost("assign-role")]
        public async Task<ActionResult<ResponseDTO>> AssignRole(
            [FromServices] IValidator<AssingRoleRequestDTO> validator,
            [FromBody] AssingRoleRequestDTO model)
        {
            var validationResult = await ValidationHelper.ValidateAsync(model, validator);
            if (validationResult != null)
                return validationResult;

            var success = await _authService.AssingRole(model.Email, model.Role!.ToUpper());

            return success
                ? Ok(new ResponseDTO())
                : BadRequest(new ResponseDTO { IsSuccess = false, Message = "Error during assigning role" });
        }
    }
}
