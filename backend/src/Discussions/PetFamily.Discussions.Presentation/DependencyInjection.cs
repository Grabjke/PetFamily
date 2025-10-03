using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Discussions.Application;
using PetFamily.Discussions.Contracts;
using PetFamily.Discussions.Infrastructure;

namespace PetFamily.Discussions.Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddDiscussionModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddDiscussionPresentation()
            .AddDiscussionApplication()
            .AddDiscussionInfrastructure(configuration);
        
        return services;
    }

    private static IServiceCollection AddDiscussionPresentation(
        this IServiceCollection services)
    {
        services.AddScoped<IDiscussionContract, DiscussionContract>();

        return services;
    }
}