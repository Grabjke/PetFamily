using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects;

namespace PetFamily.Volunteers.Application.Volunteers.Commands.ChangeStatusPet;

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
        
        var changeResult = volunteerResult.Value
            .ChangeStatusPet(PetId.Create(command.PetId), command.Status);
        if (changeResult.IsFailure)
            return changeResult.Error.ToErrorList();

        await _volunteersRepository.Save(volunteerResult.Value, cancellationToken);

        _logger.LogInformation("Successfully changed status pet with id:{PetId}",
            command.PetId);


        return command.PetId;
    }
}