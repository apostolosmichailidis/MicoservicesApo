using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using MMLib.SwaggerForOcelot;

var builder = WebApplication.CreateBuilder(args);

// Load ocelot.json
builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);

// Swagger for Gateway
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// SwaggerForOcelot
builder.Services.AddSwaggerForOcelot(builder.Configuration);

// Ocelot
builder.Services.AddOcelot(builder.Configuration);

var app = builder.Build();

// Swagger UI for Gateway
app.UseSwagger();
app.UseSwaggerUI();

// SwaggerForOcelot UI
app.UseSwaggerForOcelotUI(opt =>
{
    opt.PathToSwaggerGenerator = "/swagger/docs";
});

// Ocelot middleware
await app.UseOcelot();

app.Run();
