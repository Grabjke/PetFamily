using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Core;
using PetFamily.Core.Abstractions;
using PetFamily.Files.Application.Files.Upload;
using PetFamily.SharedKernel;

namespace PetFamily.Files.Application.Files.Remove;

public class RemoveFileHandler:ICommandHandler<List<string>,RemoveFilesCommand>
{
    private readonly IFileProvider _provider;
    private readonly ILogger<UploadFilesHandler> _logger;

    public RemoveFileHandler(IFileProvider provider, ILogger<UploadFilesHandler> logger)
    {
        _provider = provider;
        _logger = logger;
    }

    public async Task<Result<List<string>, ErrorList>> Handle(
        RemoveFilesCommand command,
        CancellationToken cancellationToken = default)
    {
        var result = await _provider.RemoveFiles(command.FilesNames, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToErrorList();

        _logger.LogInformation("Removing {Count} files.", result.Value.Count);

        return result.Value.ToList();
    }
}