using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Core;
using PetFamily.Core.Abstractions;
using PetFamily.SharedKernel;
using FileInfo = PetFamily.Core.FileProvider.FileInfo;

namespace PetFamily.Files.Application.Files.Upload;

public class UploadFilesHandler:ICommandHandler<List<string>,UploadFilesCommand>
{
    private readonly IFileProvider _provider;
    private readonly ILogger<UploadFilesHandler> _logger;
    private const string BUCKET_NAME = "photos";

    public UploadFilesHandler(IFileProvider provider, ILogger<UploadFilesHandler> logger)
    {
        _provider = provider;
        _logger = logger;
    }

    public async Task<Result<List<string>, ErrorList>> Handle(
        UploadFilesCommand command,
        CancellationToken cancellationToken = default)
    {
        List<FileData> files = [];

        foreach (var file in command.files)
        {
            var extension = Path.GetExtension(file.FileName);

            var pathResult = FilePath.Create(Guid.NewGuid(), extension);
            if (pathResult.IsFailure)
                return pathResult.Error.ToErrorList();

            var fileData = new FileData(file.Content,new FileInfo(pathResult.Value, BUCKET_NAME));

            files.Add(fileData);
        }

        var uploadFileResult = await _provider.UploadFiles(files, cancellationToken);
        if (uploadFileResult.IsFailure)
            return uploadFileResult.Error.ToErrorList();

        _logger.LogInformation("Files upload completed");

        var filePaths = files.Select(f => f.Info.FilePath.Path).ToList();

        return filePaths;
    }
}