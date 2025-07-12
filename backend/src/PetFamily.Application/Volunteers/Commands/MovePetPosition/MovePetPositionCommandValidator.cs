using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared;
using PetFamily.Domain.ValueObjects.Pet;

namespace PetFamily.Application.Volunteers.Commands.MovePetPosition;

public class MovePetPositionCommandValidator: AbstractValidator<MovePetPositionCommand>
{
    public MovePetPositionCommandValidator()
    {
        RuleFor(p => p.VolunteerId)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired());
        
        RuleFor(p => p.PetId)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired());

        RuleFor(p => p.Position)
            .MustBeValueObject(Position.Create);
    }
}