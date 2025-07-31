using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.SharedKernel;

namespace PetFamily.Volunteers.Application.Volunteers.Commands.Delete.Soft;

public class SoftDeleteVolunteerCommandValidator:AbstractValidator<SoftDeleteVolunteerCommand>
{
    public SoftDeleteVolunteerCommandValidator()
    {
        RuleFor(d => d.VolunteerId)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired());
    }
}