using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.Command.AddPhotoPet;

public class AddPhotoPetValidator: AbstractValidator<AddPhotoPetCommand>
{
    public AddPhotoPetValidator()
    {
        RuleFor(p => p.VolunteerId)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired());
        
        RuleFor(p => p.PetId)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired());
    }
    
}