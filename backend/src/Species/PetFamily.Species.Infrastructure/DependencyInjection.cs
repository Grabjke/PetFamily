using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core;
using PetFamily.Species.Application.Species;
using PetFamily.Species.Infrastructure.DbContexts;


namespace PetFamily.Species.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddSpeciesInfrastructure(
        this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddDbContexts(configuration)
            .AddRepositories()
            .AddDatabase();
       
        return services;
    }
    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<ISpeciesRepository, SpeciesRepository>();
        return services;
    }
    private static IServiceCollection AddDatabase(this IServiceCollection services)
    {
        services.AddKeyedScoped<IUnitOfWork, UnitOfWork>(Modules.Species);
        
        return services;
    }
    private static IServiceCollection AddDbContexts(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<WriteSpeciesDbContext>(_ => 
            new WriteSpeciesDbContext(configuration.GetConnectionString(InfrastructureConstants.DATABASE)!));
        
        services.AddScoped<ISpeciesReadDbContext, SpeciesReadDbContext>(_ => 
            new SpeciesReadDbContext(configuration.GetConnectionString(InfrastructureConstants.DATABASE)!));
        
        return services;
    }
}