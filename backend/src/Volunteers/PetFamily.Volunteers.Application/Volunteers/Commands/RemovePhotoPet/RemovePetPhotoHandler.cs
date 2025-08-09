using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Core;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.Core.ValueObjects.Pet;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects;

namespace PetFamily.Volunteers.Application.Volunteers.Commands.RemovePhotoPet;

public class RemovePetPhotoHandler : ICommandHandler<List<string>, RemovePetPhotoCommand>
{
    private readonly IValidator<RemovePetPhotoCommand> _validator;
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly ILogger<RemovePetPhotoHandler> _logger;
    private readonly IFileProvider _fileProvider;

    public RemovePetPhotoHandler(
        IValidator<RemovePetPhotoCommand> validator,
        IVolunteersRepository volunteersRepository,
        ILogger<RemovePetPhotoHandler> logger,
        IFileProvider fileProvider)
    {
        _validator = validator;
        _volunteersRepository = volunteersRepository;
        _logger = logger;
        _fileProvider = fileProvider;
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

        var result = await _fileProvider.RemoveFiles(
            command.PhotoNames,
            cancellationToken);
        if (result.IsFailure)
            return result.Error.ToErrorList();

        foreach (var name in command.PhotoNames)
        {
            var photoPathResult = FilePath.Create(name);
            if (photoPathResult.IsFailure)
                return photoPathResult.Error.ToErrorList();

            var photo = new Photo(photoPathResult.Value);

            var removeResult = volunteerResult.Value
                .RemovePhotoPet(PetId.Create(command.PetId), photo);
            if (removeResult.IsFailure)
                return removeResult.Error.ToErrorList();
        }

        await _volunteersRepository.Save(volunteerResult.Value, cancellationToken);

        _logger.LogInformation("Removing {Count} photos.", result.Value.Count);

        return result.Value.ToList();
    }
}