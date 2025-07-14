using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.Commands.SetMainPhotoPet;

public class SetMainPhotoPetCommandValidator:AbstractValidator<SetMainPhotoPetCommand>
{
    public SetMainPhotoPetCommandValidator()
    {
        RuleFor(p => p.VolunteerId)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired());
        
        RuleFor(p => p.PetId)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired());
        
        RuleFor(p => p.PhotoPath)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired());
    }
}