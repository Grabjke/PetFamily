using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.SharedKernel;

namespace PetFamily.Volunteers.Application.Volunteers.Commands.ChangeStatusPet;

public class ChangeStatusPetCommandValidator: AbstractValidator<ChangeStatusPetCommand>
{
    public ChangeStatusPetCommandValidator()
    {
        RuleFor(p => p.Status)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired());
    }
}