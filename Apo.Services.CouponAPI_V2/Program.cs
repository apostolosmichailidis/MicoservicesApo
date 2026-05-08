using Apo.Service.CouponAPI_v2.Mapping;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Apo.Services.CouponAPI_V2.Infrastructure;
using Apo.Services.CouponAPI_V2.Application;
using Apo.Services.CouponAPI_V2.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ICouponRepository, CouponRepository>();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

IMapper mapper = MappingConfig.RegisterMappings().CreateMapper();
builder.Services.AddSingleton(mapper);

builder.Services.AddApplication();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Exception handling middleware must be first
app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

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
