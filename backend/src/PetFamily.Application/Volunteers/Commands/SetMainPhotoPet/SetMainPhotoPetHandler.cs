using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.Commands.SetMainPhotoPet;

public class SetMainPhotoPetHandler:ICommandHandler<Guid,SetMainPhotoPetCommand>
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly IValidator<SetMainPhotoPetCommand> _validator;
    private readonly ILogger<SetMainPhotoPetHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public SetMainPhotoPetHandler(
        IVolunteersRepository volunteersRepository,
        IValidator<SetMainPhotoPetCommand> validator,
        ILogger<SetMainPhotoPetHandler> logger,
        IUnitOfWork unitOfWork)
    {
        _volunteersRepository = volunteersRepository;
        _validator = validator;
        _logger = logger;
        _unitOfWork = unitOfWork;
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

        var pet = volunteerResult.Value.Pets
            .FirstOrDefault(p => p.Id.Value == command.PetId);
        if (pet is null)
            return Errors.General.NotFound(command.PetId).ToErrorList();

        var setMainPhotoResult =  pet.SetMainPhoto(command.PhotoPath);
        if (setMainPhotoResult.IsFailure)
            return setMainPhotoResult.Error.ToErrorList();
        
        await _volunteersRepository.Save(volunteerResult.Value, cancellationToken);
        
        _logger.LogInformation("Photo with path:{Path} is main photo", command.PhotoPath);
        
        return pet.Id.Value;
    }
}