using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.ValueObjects.Pet;
using PetFamily.Domain.ValueObjects.Volunteer;

namespace PetFamily.Application.Volunteers.CreateVolunteer;

public class CreateVolunteerCommandValidator:AbstractValidator<CreateVolunteerCommand>
{
    public CreateVolunteerCommandValidator()
    {
        RuleFor(c =>c.FullName)
            .MustBeValueObject(c => FullName.Create(c.Name, c.Surname, c.Patronymic!));
        
        RuleFor(c => c.Description).MustBeValueObject(VolunteerDescription.Create);
        
        RuleFor(c => c.Experience).MustBeValueObject(VolunteerExperience.Create);
        
        RuleFor(c => c.Email).MustBeValueObject(Email.Create);
        
        RuleFor(c => c.PhoneNumber).MustBeValueObject(OwnersPhoneNumber.Create);

        RuleForEach(c => c.Requisites)
            .MustBeValueObject(r => Requisites.Create(r.Title, r.Description));
        
        RuleForEach(c => c.SocialNetworks)
            .MustBeValueObject(s => SocialNetwork.Create(s.Url, s.Name));



    }
}