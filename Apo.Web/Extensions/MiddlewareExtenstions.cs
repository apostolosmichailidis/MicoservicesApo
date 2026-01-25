using Apo.Web.Utility;

namespace Apo.Web.Extensions
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseJwtCookieToHeader(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<JwtCookieToHeaderMiddleware>();
        }
    }

    public class JwtCookieToHeaderMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtCookieToHeaderMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Δες αν υπάρχει το cookie
            if (context.Request.Cookies.TryGetValue(SD.TokenCookie, out var token))
            {
                // Αν δεν υπάρχει ήδη Authorization header, πρόσθεσέ το
                if (!context.Request.Headers.ContainsKey("Authorization"))
                {
                    context.Request.Headers.Add("Authorization", $"Bearer {token}");
                }
            }

            await _next(context);
        }
    }

}
