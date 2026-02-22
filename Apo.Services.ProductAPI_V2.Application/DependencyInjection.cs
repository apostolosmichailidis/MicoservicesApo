using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Apo.Services.ProductAPI_V2.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Fix: Use the correct method to register MediatR services
        services.AddMediatR(Assembly.GetExecutingAssembly());

        return services;
    }
}
