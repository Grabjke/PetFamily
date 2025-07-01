using Microsoft.AspNetCore.Mvc;
using Minio;
using PetFamily.API.Extensions;
using PetFamily.API.Processors;
using PetFamily.Application.Files.Presigned;
using PetFamily.Application.Files.Remove;
using PetFamily.Application.Files.Upload;

namespace PetFamily.API.Controllers.Files;

[ApiController]
[Route("[controller]")]
public class FileController : ControllerBase
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