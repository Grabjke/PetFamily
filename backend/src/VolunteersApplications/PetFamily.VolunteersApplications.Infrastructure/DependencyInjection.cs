using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core;
using PetFamily.VolunteersApplications.Application.VolunteersApplications;
using PetFamily.VolunteersApplications.Infrastructure.DbContexts;
using PetFamily.VolunteersApplications.Infrastructure.Repositories;

namespace PetFamily.VolunteersApplications.Infrastructure;

public static class DependencyInjectionInfrastructure
{
    public static IServiceCollection AddVolunteerApplicationInfrastructure(
        this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddDbContexts(configuration)
            .AddRepositories()
            .AddDatabase();

        return services;
    }

    private static IServiceCollection AddDbContexts(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<IApplicationReadDbContext, ReadApplicationDbContext>(_ =>
            new ReadApplicationDbContext(configuration.GetConnectionString(
                InfrastructureConstants.DATABASE)!));

        services.AddScoped<WriteApplicationDbContext>(_ =>
            new WriteApplicationDbContext(configuration.GetConnectionString(
                InfrastructureConstants.DATABASE)!));

        return services;
    }

    private static IServiceCollection AddDatabase(this IServiceCollection services)
    {
        services.AddKeyedScoped<IUnitOfWork, UnitOfWork>(Modules.Application);

        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IVolunteerApplicationsRepository, VolunteerApplicationsRepository>();

        return services;
    }
}