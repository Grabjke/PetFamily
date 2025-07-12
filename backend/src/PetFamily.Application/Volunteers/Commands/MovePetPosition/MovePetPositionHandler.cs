using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Extensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.ValueObjects.Pet;

namespace PetFamily.Application.Volunteers.Commands.MovePetPosition;

public class MovePetPositionHandler:ICommandHandler<Guid,MovePetPositionCommand>
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly IValidator<MovePetPositionCommand> _validator;
    private readonly ILogger<MovePetPositionHandler> _logger;

    public MovePetPositionHandler(
        IVolunteersRepository volunteersRepository,
        IValidator<MovePetPositionCommand> validator,
        ILogger<MovePetPositionHandler> logger)
    {
        _volunteersRepository = volunteersRepository;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        MovePetPositionCommand command,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
        {
            return validationResult.ToErrorList();
        }

        var volunteerResult = await _volunteersRepository
            .GetById(command.VolunteerId, cancellationToken);

        if (volunteerResult.IsFailure)
            return volunteerResult.Error.ToErrorList();

        var petId = PetId.Create(command.PetId);

        var petResult = volunteerResult.Value.GetPetById(petId);
        if(petResult.IsFailure)
            return petResult.Error.ToErrorList();
        
        var position = Position.Create(command.Position).Value;
        
        var movePetResult =volunteerResult.Value.MovePet(petResult.Value,position);
        if(movePetResult.IsFailure)
            return movePetResult.Error.ToErrorList();
        
        await _volunteersRepository.Save(volunteerResult.Value, cancellationToken);

        _logger.LogInformation("Moving pet {PetId} to position {Position}", command.PetId, position);
        
        return petId.Value;
    }
}