using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.Command.RemovePhotoPet;

public class RemovePetPhotoCommandValidator:AbstractValidator<RemovePetPhotoCommand>
{
    public RemovePetPhotoCommandValidator()
    {
        RuleFor(p => p.VolunteerId)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired());
        
        RuleFor(p => p.PetId)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired());
    }
}