using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using PetFamily.Accounts.Presentation.DependencyInjection;
using PetFamily.Core.Caching;
using PetFamily.Discussions.Presentation;
using PetFamily.Files.Application;
using PetFamily.Species.Presentation.DependencyInjection;
using PetFamily.Volunteers.Presentation.DependencyInjection;
using PetFamily.VolunteersApplications.Presentation;

namespace PetFamily.Web;

public static class DependencyInjection
{
    public static IServiceCollection AddProgramDependency(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddDiscussionModule(configuration)
            .AddVolunteersModule(configuration)
            .AddSpeciesModule(configuration)
            .AddAccountsModule(configuration)
            .AddVolunteersApplicationModule(configuration)
            .AddFilesApplication()
            .AddCore()
            .AddAppMetrics(configuration);
        
        Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

        return services;
    }

    private static IServiceCollection AddCore(this IServiceCollection services)
    {
        services.AddScoped<ICacheService, DistributedCacheService>();
        return services;
    }
    
    private static IServiceCollection AddAppMetrics(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOpenTelemetry()
            .WithMetrics(b => b
                .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("PetFamily.Web"))
                .AddMeter("PetFamily")
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddRuntimeInstrumentation()
                .AddProcessInstrumentation()
                .AddPrometheusExporter());

        return services;
    }
}