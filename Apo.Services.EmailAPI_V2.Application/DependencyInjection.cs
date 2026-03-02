using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Apo.Services.EmailAPI_V2.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(Assembly.GetExecutingAssembly());
        return services;
    }
}
