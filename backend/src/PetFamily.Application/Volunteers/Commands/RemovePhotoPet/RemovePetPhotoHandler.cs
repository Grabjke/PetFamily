using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Extensions;
using PetFamily.Application.FileProvider;
using PetFamily.Domain.Shared;
using PetFamily.Domain.ValueObjects.Pet;

namespace PetFamily.Application.Volunteers.Commands.RemovePhotoPet;

public class RemovePetPhotoHandler:ICommandHandler<List<string>,RemovePetPhotoCommand>
{
    private readonly IFileProvider _provider;
    private readonly IValidator<RemovePetPhotoCommand> _validator;
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly ILogger<RemovePetPhotoHandler> _logger;

    public RemovePetPhotoHandler(
        IFileProvider provider,
        IValidator<RemovePetPhotoCommand> validator,
        IVolunteersRepository volunteersRepository,
        ILogger<RemovePetPhotoHandler> logger)
    {
        _provider = provider;
        _validator = validator;
        _volunteersRepository = volunteersRepository;
        _logger = logger;
    }

    public async Task<Result<List<string>, ErrorList>> Handle(
        RemovePetPhotoCommand command,
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

        var result = await _provider.RemoveFiles(command.PhotoNames, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToErrorList();


        foreach (var name in command.PhotoNames)
        {
            var photoPathResult = FilePath.Create(name);
            if (photoPathResult.IsFailure)
                return photoPathResult.Error.ToErrorList();

            var photo = new Photo(photoPathResult.Value);
            
            petResult.Value.RemovePhoto(photo);
        }
        
        await _volunteersRepository.Save(volunteerResult.Value, cancellationToken);

        _logger.LogInformation("Removing {Count} photos.", result.Value.Count);

        return result.Value.ToList();
    }
}