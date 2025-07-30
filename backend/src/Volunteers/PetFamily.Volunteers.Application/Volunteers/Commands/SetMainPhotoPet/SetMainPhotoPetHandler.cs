using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Core;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects;

namespace PetFamily.Volunteers.Application.Volunteers.Commands.SetMainPhotoPet;

public class SetMainPhotoPetHandler : ICommandHandler<Guid, SetMainPhotoPetCommand>
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly IValidator<SetMainPhotoPetCommand> _validator;
    private readonly ILogger<SetMainPhotoPetHandler> _logger;

    public SetMainPhotoPetHandler(
        IVolunteersRepository volunteersRepository,
        IValidator<SetMainPhotoPetCommand> validator,
        ILogger<SetMainPhotoPetHandler> logger)
    {
        _volunteersRepository = volunteersRepository;
        _validator = validator;
        _logger = logger;
    }


    public async Task<Result<Guid, ErrorList>> Handle(
        SetMainPhotoPetCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();

        var volunteerResult = await _volunteersRepository
            .GetById(command.VolunteerId, cancellationToken);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error.ToErrorList();
        
        var petId = PetId.Create(command.PetId);
        
        var setMainPhotoResult = volunteerResult.Value
            .SetMainPhoto(petId, command.PhotoPath);
        
        if (setMainPhotoResult.IsFailure)
            return setMainPhotoResult.Error.ToErrorList();

        await _volunteersRepository.Save(volunteerResult.Value, cancellationToken);

        _logger.LogInformation("Photo with path:{Path} is main photo", command.PhotoPath);

        return petId.Value;
    }
}