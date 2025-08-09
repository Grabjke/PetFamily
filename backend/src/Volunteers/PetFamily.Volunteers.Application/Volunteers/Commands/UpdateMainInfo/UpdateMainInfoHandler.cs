using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.Core.ValueObjects.Volunteer;
using PetFamily.SharedKernel;

namespace PetFamily.Volunteers.Application.Volunteers.Commands.UpdateMainInfo;

public class UpdateMainInfoHandler : ICommandHandler<Guid, UpdateMainInfoCommand>
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly IValidator<UpdateMainInfoCommand> _validator;
    private readonly ILogger<UpdateMainInfoHandler> _logger;

    public UpdateMainInfoHandler(
        IVolunteersRepository volunteersRepository,
        IValidator<UpdateMainInfoCommand> validator,
        ILogger<UpdateMainInfoHandler> logger)
    {
        _logger = logger;
        _validator = validator;
        _volunteersRepository = volunteersRepository;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        UpdateMainInfoCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();

        var volunteerResult = await _volunteersRepository.GetById(command.VolunteerId, cancellationToken);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error.ToErrorList();

        var fullName = FullName.Create(
            command.FullName.Name,
            command.FullName.Surname,
            command.FullName.Patronymic).Value;

        var email = Email.Create(command.Email).Value;

        var description = VolunteerDescription.Create(command.Description).Value;

        var experience = VolunteerExperience.Create(command.Experience).Value;

        var phoneNumber = OwnersPhoneNumber.Create(command.PhoneNumber).Value;

        volunteerResult.Value.UpdateMainInfo(
            fullName,
            email,
            description,
            experience,
            phoneNumber);

        await _volunteersRepository.Save(volunteerResult.Value, cancellationToken);

        _logger.LogInformation("Upload volunteer with id {Id}", command.VolunteerId);

        return command.VolunteerId;
    }
}