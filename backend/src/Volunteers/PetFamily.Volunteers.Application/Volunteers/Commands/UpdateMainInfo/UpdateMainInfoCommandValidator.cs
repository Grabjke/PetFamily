using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.Core.ValueObjects.Volunteer;
using PetFamily.SharedKernel;

namespace PetFamily.Volunteers.Application.Volunteers.Commands.UpdateMainInfo;

public class UpdateMainInfoCommandValidator : AbstractValidator<UpdateMainInfoCommand>
{
    public UpdateMainInfoCommandValidator()
    {
        RuleFor(u => u.VolunteerId)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired());
        
        RuleFor(c =>c.FullName)
            .MustBeValueObject(c => FullName.Create(c.Name, c.Surname, c.Patronymic!));
        
        RuleFor(c => c.Description).MustBeValueObject(VolunteerDescription.Create);
        
        RuleFor(c => c.Experience).MustBeValueObject(VolunteerExperience.Create);
        
        RuleFor(c => c.Email).MustBeValueObject(Email.Create);
        
        RuleFor(c => c.PhoneNumber).MustBeValueObject(OwnersPhoneNumber.Create);
    }
}