using BuberDinner.Api.Common.Mapping;
using Microsoft.Extensions.DependencyInjection;

namespace BuberDinner.Application;

public static class DependencyInjection 
{
    public static IServiceCollection AddPresentation(this IServiceCollection services) 
    {
        services.AddControllers();
        services.AddMappings();
        return services;
    }
}