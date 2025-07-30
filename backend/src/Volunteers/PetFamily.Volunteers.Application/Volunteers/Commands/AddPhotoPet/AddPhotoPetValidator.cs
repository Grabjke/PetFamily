using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.SharedKernel;

namespace PetFamily.Volunteers.Application.Volunteers.Commands.AddPhotoPet;

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