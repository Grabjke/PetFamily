using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.Core.ValueObjects.Pet;
using PetFamily.Core.ValueObjects.Volunteer;
using PetFamily.SharedKernel;

namespace PetFamily.Volunteers.Application.Volunteers.Commands.UpdateMainInfoPet;

public class UpdateMainInfoPetCommandValidator: AbstractValidator<UpdateMainInfoPetCommand>
{
    public UpdateMainInfoPetCommandValidator()
    {
        RuleFor(p => p.VolunteerId)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired());
        RuleFor(p => p.PetId)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired());
        
        RuleFor(p => p.SpeciesId)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired());
        
        RuleFor(p => p.BreedId)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired());
        
        RuleFor(p=>p.Name).MustBeValueObject(PetName.Create);
        RuleFor(p=>p.Address).
            MustBeValueObject(c=>Address.Create(c.Street,c.City,c.Country,c.ZipCode));
        RuleFor(p=>p.Colour).MustBeValueObject(Colour.Create);
        RuleFor(p=>p.HealthInformation).MustBeValueObject(HealthInformation.Create);
        RuleFor(p=>p.Height).MustBeValueObject(Height.Create);
        RuleFor(p=>p.Weight).MustBeValueObject(Weight.Create);
        RuleFor(p=>p.Birthday).MustBeValueObject(Birthday.Create);
        RuleFor(p=>p.OwnersPhoneNumber).MustBeValueObject(OwnersPhoneNumber.Create);
        RuleFor(p=>p.Description).MustBeValueObject(Description.Create);
    }
}