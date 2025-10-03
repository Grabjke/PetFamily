using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Accounts.Contracts;
using PetFamily.Accounts.Contracts.Requests;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.Core.ValueObjects.VolunteerApplication;
using PetFamily.SharedKernel;
using PetFamily.VolunteersApplications.Domain.ApplicationManagement;

namespace PetFamily.VolunteersApplications.Application.VolunteersApplications.Commands.Create;

public class CreateApplicationHandler : ICommandHandler<Guid, CreateApplicationCommand>
{
    private readonly IValidator<CreateApplicationCommand> _validator;
    private readonly IVolunteerApplicationsRepository _repository;
    private readonly ILogger<CreateApplicationHandler> _logger;
    private readonly IAccountContract _accountContract;

    public CreateApplicationHandler(
        IValidator<CreateApplicationCommand> validator,
        IVolunteerApplicationsRepository repository,
        ILogger<CreateApplicationHandler> logger,
        IAccountContract accountContract)
    {
        _validator = validator;
        _repository = repository;
        _logger = logger;
        _accountContract = accountContract;
    }

    public async Task<Result<Guid, ErrorList>> Handle(CreateApplicationCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();

        var bannedResult = await _accountContract.CanSubmitApplication(
            new CanSubmitApplicationRequest(command.UserId), cancellationToken);
        
        if (bannedResult.IsFailure)
            return bannedResult.Error.ToErrorList();
        
        var volunteerInfo = VolunteerInfo.Create(
            command.VolunteerInfo.FirstName,
            command.VolunteerInfo.LastName,
            command.VolunteerInfo.PhoneNumber,
            command.VolunteerInfo.Email,
            command.VolunteerInfo.Surname,
            command.VolunteerInfo.Description).Value;

        var volunteerApplication = VolunteerApplication.Create(
            command.UserId,
            volunteerInfo).Value;

        await _repository.AddApplication(volunteerApplication, cancellationToken);

        _logger.LogInformation("Created application with id {ApplicationId}", volunteerApplication.Id);

        return volunteerApplication.Id;
    }
}