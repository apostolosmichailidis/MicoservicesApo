using System.Security.Cryptography;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Apo.Gateway.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddJwtAuthenticationForAssymetricES256(
            this IServiceCollection services,
            JwtOptionsForAssymetricES256 jwtOptions)
        {
            // Load public key
            var publicKeyPem = File.ReadAllText(jwtOptions.PublicKeyPath);
            var ecdsa = ECDsa.Create();
            ecdsa.ImportFromPem(publicKeyPem);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtOptions.Issuer,
                        ValidAudience = jwtOptions.Audience,
                        IssuerSigningKey = new ECDsaSecurityKey(ecdsa)
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = ctx => {
                            Console.WriteLine(ctx.Exception); return Task.CompletedTask;
                        }
                    };
                });

            return services;
        }
    }
}
