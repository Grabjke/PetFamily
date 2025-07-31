using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Volunteers.Application;
using PetFamily.Volunteers.Contracts;
using PetFamily.Volunteers.Infrastructure;

namespace PetFamily.Volunteers.Presentation.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddVolunteersModule(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        services
            .AddVolunteerInfrastructure(configuration)
            .AddVolunteerApplication()
            .AddVolunteerPresentation();
        
        return services;
    }

    private static IServiceCollection AddVolunteerPresentation(this IServiceCollection services)
    {
        services
            .AddContracts();
        
        return services;
    }
    
    private static IServiceCollection AddContracts(this IServiceCollection services)
    {
        return services.AddScoped<IVolunteerContract, VolunteerContract>();
    }
    
}