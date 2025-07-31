namespace PetFamily.Core.FileProvider;

public interface IFileCleanerService
{
    Task Process(CancellationToken stoppingToken);
}