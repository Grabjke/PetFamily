using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Extensions;
using PetFamily.Application.FileProvider;
using PetFamily.Domain.Shared;
using PetFamily.Domain.ValueObjects.Pet;

namespace PetFamily.Application.Volunteers.AddPhotoPet;

public class AddPhotoPetHandler
{
    private readonly IFileProvider _provider;
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly IValidator<AddPhotoPetCommand> _validator;
    private readonly ILogger<AddPhotoPetHandler> _logger;
    private const string BUCKET_NAME = "photos";

    public AddPhotoPetHandler(
        IFileProvider provider,
        IVolunteersRepository volunteersRepository,
        IValidator<AddPhotoPetCommand> validator,
        ILogger<AddPhotoPetHandler> logger)
    {
        _provider = provider;
        _volunteersRepository = volunteersRepository;
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

        var petId = PetId.Create(command.PetId);

        var petResult = volunteerResult.Value.GetPetById(petId);
        if (petResult.IsFailure)
            return petResult.Error.ToErrorList();
        
        List<FileData> files = [];

        foreach (var file in command.Files)
        {
            var extension = Path.GetExtension(file.FileName);

            var pathResult = FilePath.Create(Guid.NewGuid(), extension);
            if (pathResult.IsFailure)
                return pathResult.Error.ToErrorList();

            var fileData = new FileData(file.Content, pathResult.Value, BUCKET_NAME);

            files.Add(fileData);
        }

        var uploadFileResult = await _provider.UploadFiles(files, cancellationToken);
        if (uploadFileResult.IsFailure)
            return uploadFileResult.Error.ToErrorList();

        var filePaths = files.Select(f => f.FilePath.Path).ToList();

        foreach (var filePath in filePaths)
        {
            var filePathResult = FilePath.Create(filePath);
            if (filePathResult.IsFailure)
                return filePathResult.Error.ToErrorList();

            var photo = new Photo(filePathResult.Value);

            petResult.Value.AddPhoto(photo);
        }

        await _volunteersRepository.Save(volunteerResult.Value, cancellationToken);

        _logger.LogInformation("Upload photos success to pet with id:{petId} ", command.PetId);

        return filePaths;
    }
}