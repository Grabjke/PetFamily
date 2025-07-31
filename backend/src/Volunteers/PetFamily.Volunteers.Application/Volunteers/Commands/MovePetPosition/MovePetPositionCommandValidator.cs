using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.SharedKernel;
using PetFamily.Volunteers.Domain.PetManagement.ValueObjects.Pet;

namespace PetFamily.Volunteers.Application.Volunteers.Commands.MovePetPosition;

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