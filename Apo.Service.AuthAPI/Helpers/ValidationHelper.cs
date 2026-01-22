using Apo.Service.AuthAPI.Models.Dto;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Apo.Service.AuthAPI.Helpers
{
    public static class ValidationHelper
    {
        public static async Task<ActionResult<ResponseDTO>?> ValidateAsync<T>(
            T model,
            IValidator<T> validator)
        {
            var result = await validator.ValidateAsync(model);

            if (result.IsValid)
                return null;

            return new BadRequestObjectResult(new ResponseDTO
            {
                IsSuccess = false,
                Message = "Validation failed",
                Errors = result.Errors.Select(e => e.ErrorMessage).ToList()
            });
        }
    }
}
