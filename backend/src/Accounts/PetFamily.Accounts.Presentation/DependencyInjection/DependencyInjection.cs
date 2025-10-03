using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Accounts.Application;
using PetFamily.Accounts.Contracts;
using PetFamily.Accounts.Infrastructure;

namespace PetFamily.Accounts.Presentation.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddAccountsModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddAccountsInfrastructure(configuration)
            .AddAccountsApplication()
            .AddAccountsPresentation();

        return services;
    }

    private static IServiceCollection AddAccountsPresentation(
        this IServiceCollection services)
    {
        services.AddScoped<IAccountContract, AccountsContract>();

        return services;
    }
}