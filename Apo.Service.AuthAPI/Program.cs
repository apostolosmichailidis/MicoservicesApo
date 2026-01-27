using Apo.Service.AuthAPI.Extensions;
using Apo.Service.AuthAPI.Models;
using Apo.Service.AuthAPI.Service;
using Apo.Service.AuthAPI.Service.IService;
using Apo.Services.AuthAPI.Data;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

//SymmetricAlgorithm HMAC SHA256 Authentication
//builder.Services.Configure<JwtOptionsForSymmetricHmacSha256>(builder.Configuration.GetSection("ApiSettings:JwtOptionsForSymmetricHmacSha256"));

//AssymetricAlgorithm ECDSA ES256 Authentication
builder.Services.Configure<JwtOptionsForAssymetricES256>(builder.Configuration.GetSection("ApiSettings:JwtOptionsForAssymetricES256"));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddControllers();

builder.Services.AddValidatorsFromAssemblyContaining<Program>();

//SymmetricAlgorithm HMAC SHA256 Authentication
//builder.Services.AddScoped<IJWTTokenGenerator, JwtTokenGeneratorUsingSymmetricHmacSha256>();

//AssymetricAlgorithm ECDSA ES256 Authentication
builder.Services.AddScoped<IJWTTokenGenerator, JwtTokenGeneratorUsingES256>();

builder.Services.AddScoped<IAuthService, AuthService>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
});

//AssymetricAlgorithm ECDSA ES256 Authentication
var JwtOptionsForAssymetricES256 = builder.Configuration.GetSection("ApiSettings:JwtOptionsForAssymetricES256").Get<JwtOptionsForAssymetricES256>();
builder.Services.AddJwtAuthenticationForAssymetricES256(JwtOptionsForAssymetricES256);
//AssymetricAlgorithm ECDSA ES256 Authentication --END--

//SymmetricAlgorithm HMAC SHA256 Authentication
//var JwtOptionsForSymmetricHmacSha256 = builder.Configuration.GetSection("ApiSettings:JwtOptionsForSymmetricHmacSha256").Get<JwtOptionsForSymmetricHmacSha256>();
//builder.Services.AddJwtAuthenticationForSymmetricHmacSha256(JwtOptionsForSymmetricHmacSha256);
//SymmetricAlgorithm HMAC SHA256 Authentication --END--

builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "Enter JWT token like: Bearer {your token}",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

ApplyMigrations();

app.Run();

void ApplyMigrations()
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    if (dbContext.Database.GetPendingMigrations().Any())
    {
        dbContext.Database.Migrate();
    }
}

//{
//  "userName": "apo@apo.com",
//  "password": "1234Aa#"
//}