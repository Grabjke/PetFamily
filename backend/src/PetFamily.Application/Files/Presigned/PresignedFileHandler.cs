using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.FileProvider;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Files.Presigned;

public class PresignedFileHandler
{
    private readonly IFileProvider _provider;
    private readonly ILogger<PresignedFileHandler> _logger;

    public PresignedFileHandler(IFileProvider provider, ILogger<PresignedFileHandler> logger)
    {
        _provider = provider;
        _logger = logger;
    }

    public async Task<Result<string, ErrorList>> Handle(
        PresignedFileCommand command,
        CancellationToken cancellationToken = default)
    {
        var result = await _provider.PresignedGetFile(command.FileName, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToErrorList();

        _logger.LogInformation("Presigned file with {name}:  .", command.FileName);

        return result.Value;
    }
}