using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Apo.Gateway.Extensions;
using Apo.Gateway;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
});

//AssymetricAlgorithm ECDSA ES256 Authentication
var jwtOptionsForAssymetricES256 = builder.Configuration.GetSection("ApiSettings:JwtOptionsForAssymetricES256").Get<JwtOptionsForAssymetricES256>();
builder.Services.AddJwtAuthenticationForAssymetricES256(jwtOptionsForAssymetricES256);
builder.Services.AddAuthorization();

builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);

builder.Services.AddOcelot(builder.Configuration);

var app = builder.Build();
await app.UseOcelot();

app.Run();
