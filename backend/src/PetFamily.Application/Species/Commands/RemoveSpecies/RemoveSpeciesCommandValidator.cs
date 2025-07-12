using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Species.Commands.RemoveSpecies;

public class RemoveSpeciesCommandValidator:AbstractValidator<RemoveSpeciesCommand>
{
    public RemoveSpeciesCommandValidator()
    {
        RuleFor(s => s.SpeciesId)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired());
        
    }
}