using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Extensions;
using PetFamily.Domain.Entities;
using PetFamily.Domain.Shared;
using PetFamily.Domain.ValueObjects.Volunteer;

namespace PetFamily.Application.Volunteers.CreateVolunteer;

public class CreateVolunteerHandler
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

        var volunteer = new Volunteer(
            volunteerId,
            fullName,
            email,
            petDescription,
            experience,
            phoneNumber
        );

        if (command.SocialNetworks?.Any() == true)
        {
            foreach (var (url, name) in command.SocialNetworks)
            {
                var socialNetwork = SocialNetwork.Create(url, name).Value;

                var addResult = volunteer.AddSocialNetwork(socialNetwork);
                if (addResult.IsFailure)
                    return addResult.Error.ToErrorList();
            }
        }

        if (command.Requisites?.Any() == true)
        {
            foreach (var (title, description) in command.Requisites)
            {
                var requisites = Requisites.Create(title, description).Value;

                var addRequisitesResult = volunteer.AddRequisites(requisites);
                if (addRequisitesResult.IsFailure)
                    return addRequisitesResult.Error.ToErrorList();
            }
        }

        await _volunteersRepository.Add(volunteer, cancellationToken);

        _logger.LogInformation("Created volunteer with id {Id}", volunteerId);

        return volunteer.Id.Value;
    }
}