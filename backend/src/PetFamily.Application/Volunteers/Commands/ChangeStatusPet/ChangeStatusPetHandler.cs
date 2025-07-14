using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Extensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.Commands.ChangeStatusPet;

public class ChangeStatusPetHandler : ICommandHandler<Guid, ChangeStatusPetCommand>
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly IValidator<ChangeStatusPetCommand> _validator;
    private readonly ILogger<ChangeStatusPetHandler> _logger;

    public ChangeStatusPetHandler(
        IVolunteersRepository volunteersRepository,
        IValidator<ChangeStatusPetCommand> validator,
        ILogger<ChangeStatusPetHandler> logger)
    {
        _volunteersRepository = volunteersRepository;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        ChangeStatusPetCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();

        var volunteerResult = await _volunteersRepository.GetById(command.VolunteerId, cancellationToken);
        if (validationResult.IsValid == false)
            return Errors.General.NotFound(command.VolunteerId).ToErrorList();

        var pet = volunteerResult.Value.Pets
            .FirstOrDefault(p => p.Id.Value == command.PetId);
        if (pet is null)
            return Errors.General.NotFound(command.PetId).ToErrorList();

        var changeResult = pet.ChangeStatus(command.Status);
        if (changeResult.IsFailure)
            return changeResult.Error.ToErrorList();

        await _volunteersRepository.Save(volunteerResult.Value, cancellationToken);

        _logger.LogInformation("Successfully changed status pet with id:{PetId}",
            command.PetId);


        return command.PetId;
    }
}