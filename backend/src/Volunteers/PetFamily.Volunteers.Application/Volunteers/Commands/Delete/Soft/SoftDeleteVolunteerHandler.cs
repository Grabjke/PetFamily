using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Core;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;

namespace PetFamily.Volunteers.Application.Volunteers.Commands.Delete.Soft;

public class SoftDeleteVolunteerHandler : ICommandHandler<Guid, SoftDeleteVolunteerCommand>
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly IValidator<SoftDeleteVolunteerCommand> _validator;
    private readonly ILogger<SoftDeleteVolunteerHandler> _logger;

    public SoftDeleteVolunteerHandler(
        IVolunteersRepository volunteersRepository,
        IValidator<SoftDeleteVolunteerCommand> validator,
        ILogger<SoftDeleteVolunteerHandler> logger)
    {
        _logger = logger;
        _validator = validator;
        _volunteersRepository = volunteersRepository;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        SoftDeleteVolunteerCommand command,
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