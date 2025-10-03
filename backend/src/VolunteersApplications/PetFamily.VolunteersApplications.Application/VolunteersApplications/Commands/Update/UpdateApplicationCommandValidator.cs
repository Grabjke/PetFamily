using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.Core.ValueObjects.VolunteerApplication;
using PetFamily.SharedKernel;

namespace PetFamily.VolunteersApplications.Application.VolunteersApplications.Commands.Update;

public class UpdateApplicationCommandValidator : AbstractValidator<UpdateApplicationCommand>
{
    public UpdateApplicationCommandValidator()
    {
        RuleFor(c => c.ApplicationId)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired());
        
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