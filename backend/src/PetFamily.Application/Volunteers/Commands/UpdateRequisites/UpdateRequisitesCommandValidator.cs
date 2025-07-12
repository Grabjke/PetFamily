using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared;
using PetFamily.Domain.ValueObjects.Volunteer;

namespace PetFamily.Application.Volunteers.Commands.UpdateRequisites;

public class UpdateRequisitesCommandValidator:AbstractValidator<UpdateRequisitesCommand>
{
    public UpdateRequisitesCommandValidator()
    {
        RuleFor(u => u.VolunteerId)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired());
        
        RuleForEach(c => c.Requisites)
            .MustBeValueObject(r => Requisites.Create(r.Title, r.Description));
    }
}