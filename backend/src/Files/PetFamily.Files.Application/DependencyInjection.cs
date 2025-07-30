using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstractions;

namespace PetFamily.Files.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddFilesApplication(this IServiceCollection services)
    {
        services
            .AddCommands();

        return services;
    }
    
    private static IServiceCollection AddCommands(this IServiceCollection services)
    {
        return services.Scan(scan => scan.FromAssemblies(typeof(DependencyInjection).Assembly)
            .AddClasses(classes => classes.AssignableToAny(
                [typeof(ICommandHandler<>), typeof(ICommandHandler<,>)]))
            .AsSelfWithInterfaces()
            .WithScopedLifetime());
    }
}