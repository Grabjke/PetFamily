using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.SharedKernel;

namespace PetFamily.Species.Application.Species.Commands.RemoveSpecies;

public class RemoveSpeciesCommandValidator:AbstractValidator<RemoveSpeciesCommand>
{
    public RemoveSpeciesCommandValidator()
    {
        RuleFor(s => s.SpeciesId)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired());
        
    }
}