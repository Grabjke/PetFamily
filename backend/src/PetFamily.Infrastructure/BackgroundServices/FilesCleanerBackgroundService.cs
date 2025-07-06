using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PetFamily.Application.FileProvider;


namespace PetFamily.Infrastructure.BackgroundServices;

public class FilesCleanerBackgroundService : BackgroundService
{
    private readonly ILogger<FilesCleanerBackgroundService> _logger;
    private readonly IServiceScopeFactory _scopeFactory;

    public FilesCleanerBackgroundService(
        ILogger<FilesCleanerBackgroundService> logger,
        IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Starting file cleaner");
        
        await using var scope = _scopeFactory.CreateAsyncScope();
        

        var filesCleanerService=scope.ServiceProvider.GetRequiredService<IFileCleanerService>();
        
        while (!stoppingToken.IsCancellationRequested)
        {
            await filesCleanerService.Process(stoppingToken);
        }

        await Task.CompletedTask;
    }
}