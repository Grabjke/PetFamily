using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.Commands.Delete.Soft;

public class SoftDeleteVolunteerCommandValidator:AbstractValidator<SoftDeleteVolunteerCommand>
{
    public SoftDeleteVolunteerCommandValidator()
    {
        RuleFor(d => d.VolunteerId)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired());
    }
}