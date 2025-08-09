using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.Core.ValueObjects.Volunteer;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects;

namespace PetFamily.Volunteers.Application.Volunteers.Commands.Create;

public class CreateVolunteerHandler:ICommandHandler<Guid,CreateVolunteerCommand>
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly IValidator<CreateVolunteerCommand> _validator;
    private readonly ILogger<CreateVolunteerHandler> _logger;

    public CreateVolunteerHandler(
        IVolunteersRepository volunteersRepository,
        IValidator<CreateVolunteerCommand> validator,
        ILogger<CreateVolunteerHandler> logger)
    {
        _logger = logger;
        _validator = validator;
        _volunteersRepository = volunteersRepository;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        CreateVolunteerCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
        {
            return validationResult.ToErrorList();
        }

        var volunteerId = VolunteerId.NewVolunteerId();

        var fullName = FullName.Create(
            command.FullName.Name,
            command.FullName.Surname,
            command.FullName.Patronymic!).Value;

        var petDescription = VolunteerDescription.Create(command.Description).Value;

        var experience = VolunteerExperience.Create(command.Experience).Value;

        var email = Email.Create(command.Email).Value;

        var phoneNumber = OwnersPhoneNumber.Create(command.PhoneNumber).Value;

        var volunteerByNameResult = await _volunteersRepository.GetByName(
            command.FullName.Name,
            cancellationToken);
        if (volunteerByNameResult.IsSuccess)
            return Errors.General.AllReadyExist().ToErrorList();

        var volunteer = new Domain.PetManagement.Volunteer(
            volunteerId,
            fullName,
            email,
            petDescription,
            experience,
            phoneNumber
        );
        
        await _volunteersRepository.Add(volunteer, cancellationToken);

        _logger.LogInformation("Created volunteer with id {Id}", volunteerId);

        return volunteer.Id.Value;
    }
}