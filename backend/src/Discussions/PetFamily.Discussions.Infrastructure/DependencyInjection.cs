using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core;
using PetFamily.Discussions.Application.Discussions;
using PetFamily.Discussions.Infrastructure.DbContexts;
using PetFamily.Discussions.Infrastructure.Repositories;

namespace PetFamily.Discussions.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddDiscussionInfrastructure(
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
        services.AddScoped<IDiscussionReadDbContext,ReadDiscussionDbContext>(_ =>
            new ReadDiscussionDbContext(configuration.GetConnectionString(
                InfrastructureConstants.DATABASE)!));
        
        services.AddScoped<WriteDiscussionDbContext>(_ =>
            new WriteDiscussionDbContext(configuration.GetConnectionString(
                InfrastructureConstants.DATABASE)!));

        return services;
    }

    private static IServiceCollection AddDatabase(this IServiceCollection services)
    {
        services.AddKeyedScoped<IUnitOfWork, UnitOfWork>(Modules.Discussion);
        
        return services;
    }
    private static IServiceCollection AddRepositories(
        this IServiceCollection services)
    {
        services.AddScoped<IDiscussionRepository, DiscussionRepository>();

        return services;
    }
}