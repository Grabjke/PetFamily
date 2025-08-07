using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;
using PetFamily.Core;
using PetFamily.Core.FileProvider;
using PetFamily.Core.Messaging;
using PetFamily.Core.Options;
using PetFamily.Volunteers.Application.Volunteers;
using PetFamily.Volunteers.Infrastructure.BackgroundServices;
using PetFamily.Volunteers.Infrastructure.DbContexts;
using PetFamily.Volunteers.Infrastructure.Files;
using PetFamily.Volunteers.Infrastructure.MessageQueues;
using PetFamily.Volunteers.Infrastructure.Providers;
using PetFamily.Volunteers.Infrastructure.Repositories;
using FileInfo = PetFamily.Core.FileProvider.FileInfo;


namespace PetFamily.Volunteers.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddVolunteerInfrastructure(
        this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddDbContexts(configuration)
            .AddRepositories()
            .AddHostedService()
            .AddMessageQueues()
            .AddDatabase()
            .AddMinio(configuration);
            
       
        return services;
    }

    private static IServiceCollection AddMessageQueues(this IServiceCollection services)
    {
        services.AddSingleton<IMessageQueue<IEnumerable<FileInfo>>, 
            InMemoryMessageQueue<IEnumerable<FileInfo>>>();
        
        return services;
    }
    private static IServiceCollection AddHostedService(this IServiceCollection services)
    {
        services.AddHostedService<FilesCleanerBackgroundService>();
        services.AddScoped<IFileCleanerService,FilesCleanerService>();
        return services;
    }
    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IVolunteersRepository, VolunteersRepository>();
        
        return services;
    }
    private static IServiceCollection AddDatabase(this IServiceCollection services)
    {
        services.AddKeyedScoped<IUnitOfWork, UnitOfWork>(Modules.Volunteers);
        
        return services;
    }
    private static IServiceCollection AddDbContexts(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<WriteVolunteerDbContext>(_ => 
            new WriteVolunteerDbContext(configuration.GetConnectionString(
                InfrastructureConstants.DATABASE)!));
        
        services.AddScoped<IVolunteersReadDbContext, VolunteerReadDbContext>(_ => 
            new VolunteerReadDbContext(configuration.GetConnectionString(
                InfrastructureConstants.DATABASE)!));
        
        return services;
    }
    private static IServiceCollection AddMinio(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MinioOptions>(configuration.GetSection(MinioOptions.SECTION_NAME));

        services.AddMinio(options =>
        {
            var minioOptions = configuration.GetSection(MinioOptions.SECTION_NAME).Get<MinioOptions>()
                               ?? throw new ApplicationException("Minio options not found");

            options.WithEndpoint(minioOptions.Endpoint);
            options.WithCredentials(minioOptions.AccessKey, minioOptions.SecretKey);
            options.WithSSL(minioOptions.WithSsl);
        });
        
        services.AddScoped<IFileProvider, MinioProvider>();
        
        return services;
    }
}