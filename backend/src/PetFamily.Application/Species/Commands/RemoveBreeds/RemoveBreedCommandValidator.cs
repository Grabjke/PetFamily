using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Species.Commands.RemoveBreeds;

public class RemoveBreedCommandValidator:AbstractValidator<RemoveBreedCommand>
{
    public RemoveBreedCommandValidator()
    {
        RuleFor(b => b.SpeciesId)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired());
        
        RuleFor(b => b.BreedId)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired());
    }
}