using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.Commands.ChangeStatusPet;

public class ChangeStatusPetCommandValidator: AbstractValidator<ChangeStatusPetCommand>
{
    public ChangeStatusPetCommandValidator()
    {
        RuleFor(p => p.Status)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired());
    }
}