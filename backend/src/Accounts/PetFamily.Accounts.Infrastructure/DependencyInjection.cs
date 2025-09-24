using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using PetFamily.Accounts.Application;
using PetFamily.Accounts.Domain;
using PetFamily.Accounts.Infrastructure.IdentityManagers;
using PetFamily.Accounts.Infrastructure.Options;
using PetFamily.Accounts.Infrastructure.Seeding;
using PetFamily.Core;
using PetFamily.Framework;
using PetFamily.Framework.Authorization;

namespace PetFamily.Accounts.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddAccountsInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddAuthorizationDbContext(configuration)
            .AddDatabase();

        services.AddSingleton<AccountsSeeder>();
        services.AddScoped<PermissionManager>();
        services.AddScoped<IRefreshSessionManager, RefreshSessionManager>();
        services.AddScoped<RolePermissionManager>();
        services.AddScoped<AccountsManager>();
        services.AddScoped<AccountsSeederService>();
        services.AddSingleton<IAuthorizationHandler, PermissionRequirementHandler>();
        services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
        
        
        return services;
    }

    private static IServiceCollection AddAuthorizationDbContext(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddTransient<ITokenProvider, JwtTokenProvider>();

        services.Configure<JwtOptions>(
            configuration.GetSection(JwtOptions.JWT));
        
        services.Configure<RefreshSessionOptions>(
            configuration.GetSection(RefreshSessionOptions.RefreshSession));
        
        services.Configure<AdminOptions>(
            configuration.GetSection(AdminOptions.ADMIN));

        services
            .AddIdentity<User, Role>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.User.AllowedUserNameCharacters =
                    "абвгдеёжзийклмнопрстуфхцчшщъыьэюяАБВГДЕЁЖЗИЙКЛМНОПРСТУ" +
                    "ФХЦЧШЩЪЫЬЭЮЯabcdefghijklmnopqrst" +
                    "uvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            })
            .AddEntityFrameworkStores<AccountDbContext>()
            .AddDefaultTokenProviders();

        services.AddScoped<AccountDbContext>(_ =>
            new AccountDbContext(configuration.GetConnectionString(
                InfrastructureConstants.DATABASE)!)
        );

        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                var jwtOptions = configuration.GetSection(JwtOptions.JWT).Get<JwtOptions>()
                                 ?? throw new ApplicationException("Options jwt not found");
                
                options.TokenValidationParameters = TokenValidationParametersFactory
                    .CreateWithLifeTime(jwtOptions);
            });
        
        services.AddAuthorization();
        
        return services;
    }
    private static IServiceCollection AddDatabase(this IServiceCollection services)
    {
        services.AddKeyedScoped<IUnitOfWork, UnitOfWork>(Modules.Accounts);
        
        return services;
    }
}