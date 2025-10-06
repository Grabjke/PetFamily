using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.VolunteersApplications.Application;
using PetFamily.VolunteersApplications.Infrastructure;

namespace PetFamily.VolunteersApplications.Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddVolunteersApplicationModule(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        services
            .AddVolunteerApplicationInfrastructure(configuration)
            .AddVolunteerApplication();
        
        return services;
    }
}