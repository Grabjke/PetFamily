using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Extensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.Commands.Delete;

public class SoftDeleteVolunteerHandler:ICommandHandler<Guid,DeleteVolunteerCommand>
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly IValidator<DeleteVolunteerCommand> _validator;
    private readonly ILogger<SoftDeleteVolunteerHandler> _logger;

    public SoftDeleteVolunteerHandler(
        IVolunteersRepository volunteersRepository,
        IValidator<DeleteVolunteerCommand> validator,
        ILogger<SoftDeleteVolunteerHandler> logger)
    {
        _logger = logger;
        _validator = validator;
        _volunteersRepository = volunteersRepository;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        DeleteVolunteerCommand command,
        CancellationToken cancellationToken)
    {
        var commandResult = await _validator.ValidateAsync(command, cancellationToken);
        if (commandResult.IsValid == false)
            return commandResult.ToErrorList();

        var volunteerResult = await _volunteersRepository.GetById(command.VolunteerId, cancellationToken);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error.ToErrorList();
        
        volunteerResult.Value.Delete();
        
        await _volunteersRepository.Save(volunteerResult.Value, cancellationToken);

        _logger.LogInformation("Volunteer with id:{VolunteerId} soft deleted", command.VolunteerId);

        return command.VolunteerId;
    }
}