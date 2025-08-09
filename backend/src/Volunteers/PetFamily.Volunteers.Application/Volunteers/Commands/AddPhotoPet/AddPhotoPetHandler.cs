using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Core;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.Core.Messaging;
using PetFamily.Core.ValueObjects.Pet;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects;
using FileInfo = PetFamily.Core.FileProvider.FileInfo;

namespace PetFamily.Volunteers.Application.Volunteers.Commands.AddPhotoPet;

public class AddPhotoPetHandler : ICommandHandler<List<string>, AddPhotoPetCommand>
{
    private readonly IFileProvider _provider;
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly IMessageQueue<IEnumerable<FileInfo>> _messageQueue;
    private readonly IValidator<AddPhotoPetCommand> _validator;
    private readonly ILogger<AddPhotoPetHandler> _logger;
    private const string BUCKET_NAME = "photos";

    public AddPhotoPetHandler(
        IFileProvider provider,
        IVolunteersRepository volunteersRepository,
        IMessageQueue<IEnumerable<FileInfo>> messageQueue,
        IValidator<AddPhotoPetCommand> validator,
        ILogger<AddPhotoPetHandler> logger)
    {
        _provider = provider;
        _volunteersRepository = volunteersRepository;
        _messageQueue = messageQueue;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<List<string>, ErrorList>> Handle(
        AddPhotoPetCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
        {
            return validationResult.ToErrorList();
        }

        var volunteerResult = await _volunteersRepository.GetById(command.VolunteerId, cancellationToken);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error.ToErrorList();

        List<FileData> files = [];

        foreach (var file in command.Files)
        {
            var extension = Path.GetExtension(file.FileName);

            var pathResult = FilePath.Create(Guid.NewGuid(), extension);
            if (pathResult.IsFailure)
                return pathResult.Error.ToErrorList();

            var fileData = new FileData(file.Content, new FileInfo(pathResult.Value, BUCKET_NAME));

            files.Add(fileData);
        }

        var uploadFileResult = await _provider.UploadFiles(files, cancellationToken);
        if (uploadFileResult.IsFailure)
        {
            await _messageQueue.WriteAsync(files.Select(file => file.Info), cancellationToken);

            return uploadFileResult.Error.ToErrorList();
        }

        var filePaths = files.Select(f => f.Info.FilePath.Path).ToList();

        foreach (var filePath in filePaths)
        {
            var filePathResult = FilePath.Create(filePath);
            if (filePathResult.IsFailure)
                return filePathResult.Error.ToErrorList();

            var photo = new Photo(filePathResult.Value);

            volunteerResult.Value.AddPhotoPet(PetId.Create(command.PetId), photo);
        }

        await _volunteersRepository.Save(volunteerResult.Value, cancellationToken);

        _logger.LogInformation("Upload photos success to pet with id:{petId} ", command.PetId);

        return filePaths;
    }
}