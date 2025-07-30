using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.SharedKernel;

namespace PetFamily.Species.Application.Species.Commands.RemoveBreeds;

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