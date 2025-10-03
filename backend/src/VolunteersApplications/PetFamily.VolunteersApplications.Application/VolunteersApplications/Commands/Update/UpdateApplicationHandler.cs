using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.Core.ValueObjects.VolunteerApplication;
using PetFamily.SharedKernel;

namespace PetFamily.VolunteersApplications.Application.VolunteersApplications.Commands.Update;

public class UpdateApplicationHandler : ICommandHandler<Guid, UpdateApplicationCommand>
{
    private readonly IValidator<UpdateApplicationCommand> _validator;
    private readonly ILogger<UpdateApplicationHandler> _logger;
    private readonly IVolunteerApplicationsRepository _repository;

    public UpdateApplicationHandler(
        IValidator<UpdateApplicationCommand> validator,
        ILogger<UpdateApplicationHandler> logger,
        IVolunteerApplicationsRepository repository)
    {
        _validator = validator;
        _logger = logger;
        _repository = repository;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        UpdateApplicationCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();

        var applicationResult = await _repository.GetById(command.ApplicationId, cancellationToken);
        if (applicationResult.IsFailure)
            return applicationResult.Error.ToErrorList();

        var updateResult = applicationResult.Value.Update(
            command.UserId,
            VolunteerInfo.Create(
                command.VolunteerInfo.FirstName,
                command.VolunteerInfo.LastName,
                command.VolunteerInfo.PhoneNumber,
                command.VolunteerInfo.Email,
                command.VolunteerInfo.Surname,
                command.VolunteerInfo.Description).Value);

        if (updateResult.IsFailure)
            return updateResult.Error.ToErrorList();

        await _repository.Save(applicationResult.Value, cancellationToken);

        _logger.LogInformation("Updated Volunteer Application {ApplicationId}", command.ApplicationId);

        return applicationResult.Value.Id;
    }
}