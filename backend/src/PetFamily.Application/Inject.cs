using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Abstractions;
using PetFamily.Application.FileProvider;
using PetFamily.Application.Files.Presigned;
using PetFamily.Application.Files.Remove;
using PetFamily.Application.Files.Upload;
using PetFamily.Application.Volunteers.Queries.GetVolunteersWithPagination;

namespace PetFamily.Application;

public static class Inject
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services
            .AddCommands()
            .AddQueries()
            .AddValidatorsFromAssembly(typeof(Inject).Assembly);

        return services;
    }

    private static IServiceCollection AddCommands(this IServiceCollection services)
    {
        return services.Scan(scan => scan.FromAssemblies(typeof(Inject).Assembly)
            .AddClasses(classes => classes.AssignableToAny(
                [typeof(ICommandHandler<>), typeof(ICommandHandler<,>)]))
            .AsSelfWithInterfaces()
            .WithScopedLifetime());
    }
    private static IServiceCollection AddQueries(this IServiceCollection services)
    {
        return services.Scan(scan => scan.FromAssemblies(typeof(Inject).Assembly)
            .AddClasses(classes => classes
                .AssignableTo(typeof(IQueryHandler<,>)))
            .AsSelfWithInterfaces()
            .WithScopedLifetime());
    }
    
}