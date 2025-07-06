namespace PetFamily.Application.FileProvider;

public interface IFileCleanerService
{
    Task Process(CancellationToken stoppingToken);
}