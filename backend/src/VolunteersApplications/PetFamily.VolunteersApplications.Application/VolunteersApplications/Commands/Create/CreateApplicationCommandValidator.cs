using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.Core.ValueObjects.VolunteerApplication;
using PetFamily.SharedKernel;

namespace PetFamily.VolunteersApplications.Application.VolunteersApplications.Commands.Create;

public class CreateApplicationCommandValidator: AbstractValidator<CreateApplicationCommand>
{
    public CreateApplicationCommandValidator()
    {
        RuleFor(c => c.UserId)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired()); 

        RuleFor(c => c.VolunteerInfo)
            .MustBeValueObject(c => VolunteerInfo.Create(
                c.FirstName,
                c.LastName,
                c.PhoneNumber,
                c.Email,
                c.Surname,
                c.Description));
    }
}