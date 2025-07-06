using PetFamily.Application.FileProvider;
using PetFamily.Application.Messaging;
using FileInfo = PetFamily.Application.FileProvider.FileInfo;

namespace PetFamily.Infrastructure.Files;

public class FilesCleanerService : IFileCleanerService
{
    private readonly IFileProvider _fileProvider;
    private readonly IMessageQueue<IEnumerable<FileInfo>> _messageQueue;

    public FilesCleanerService(
        IFileProvider fileProvider,
        IMessageQueue<IEnumerable<FileInfo>> messageQueue)
    {
        _fileProvider = fileProvider;
        _messageQueue = messageQueue;
    }

    public async Task Process(CancellationToken stoppingToken)
    {
        var fileInfos = await _messageQueue.ReadAsync(stoppingToken);

        foreach (var fileInfo in fileInfos)
        {
            await _fileProvider.DeleteFile(fileInfo, stoppingToken);
        }
    }
    
}