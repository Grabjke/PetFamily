using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.SharedKernel;
using PetFamily.Volunteers.Domain.PetManagement.ValueObjects.Volunteer;

namespace PetFamily.Volunteers.Application.Volunteers.Commands.UpdateRequisites;

public class UpdateRequisitesCommandValidator : AbstractValidator<UpdateRequisitesCommand>
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