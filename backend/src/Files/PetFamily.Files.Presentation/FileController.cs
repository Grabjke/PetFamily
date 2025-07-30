using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Minio;
using PetFamily.Core;
using PetFamily.Files.Application.Files.Presigned;
using PetFamily.Files.Application.Files.Remove;
using PetFamily.Files.Application.Files.Upload;
using PetFamily.Framework;

namespace PetFamily.Files.Presentation;

[ApiController]
[Route("[controller]")]
public class FileController : ApplicationController
{
    private readonly IMinioClient _minioClient;

    public FileController(IMinioClient minioClient)
    {
        _minioClient = minioClient;
    }

    [HttpPost("/photos")]
    public async Task<ActionResult> UploadFiles(
        IFormFileCollection files,
        [FromServices] UploadFilesHandler handler,
        CancellationToken cancellationToken)
    {
        await using var fileProcessor = new FormFileProcessor();

        var filesDtos = fileProcessor.Process(files);

        var command = new UploadFilesCommand(filesDtos);

        var result = await handler.Handle(command, cancellationToken);

        return result.IsFailure
            ? result.Error.ToResponse()
            : Ok(result.Value);
    }

    [HttpDelete("/photos")]
    public async Task<ActionResult> RemoveFiles(
        IEnumerable<string> names,
        [FromServices] RemoveFileHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new RemoveFilesCommand(names);
        
        var result = await handler.Handle(command, cancellationToken);

        return result.IsFailure
            ? result.Error.ToResponse()
            : Ok(result.Value);
    }
    
    [HttpGet("/photo-link")]
    public async Task<ActionResult> PresignedFile(
        string fileName,
        [FromServices] PresignedFileHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new PresignedFileCommand(fileName);

        var result = await handler.Handle(command, cancellationToken);

        return result.IsFailure
            ? result.Error.ToResponse()
            : Ok(result.Value);
    }
}