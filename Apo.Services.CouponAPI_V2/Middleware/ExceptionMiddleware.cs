using Apo.Service.CouponAPI_v2.Models;
using Apo.Services.CouponAPI_V2.Application.Common.Exceptions;

namespace Apo.Services.CouponAPI_V2.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (NotFoundException ex)
            {
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                await context.Response.WriteAsJsonAsync(new ResponseDto
                {
                    IsSuccess = false,
                    Message = ex.Message
                });
            }
            catch (ValidationException ex)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsJsonAsync(new ResponseDto
                {
                    IsSuccess = false,
                    Message = ex.Message
                });
            }
            catch (Exception ex) 
            { 
                context.Response.StatusCode = StatusCodes.Status500InternalServerError; 
                await context.Response.WriteAsJsonAsync(new ResponseDto 
                { 
                    IsSuccess = false, 
                    Message = "An unexpected error occurred" 
                }); 
            }
        }
    }
}
