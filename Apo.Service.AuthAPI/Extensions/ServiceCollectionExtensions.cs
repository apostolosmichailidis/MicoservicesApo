using Apo.Service.AuthAPI.Models;
using Apo.Services.AuthAPI.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Text;

namespace Apo.Service.AuthAPI.Extensions
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
                });

            return services;
        }


        public static IServiceCollection AddJwtAuthenticationForSymmetricHmacSha256(
            this IServiceCollection services,
            JwtOptionsForSymmetricHmacSha256 jwtOptions)
        {
            var secret = Encoding.ASCII.GetBytes(jwtOptions.Secret);

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
                        IssuerSigningKey = new SymmetricSecurityKey(secret)
                    };
                });

            return services;
        }
    }
}
