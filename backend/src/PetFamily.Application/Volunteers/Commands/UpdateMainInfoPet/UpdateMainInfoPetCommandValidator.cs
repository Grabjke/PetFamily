using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared;
using PetFamily.Domain.ValueObjects.Pet;
using PetFamily.Domain.ValueObjects.Volunteer;

namespace PetFamily.Application.Volunteers.Commands.UpdateMainInfoPet;

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
        RuleFor(p=>p.Colour).MustBeValueObject(PetColour.Create);
        RuleFor(p=>p.HealthInformation).MustBeValueObject(PetHealthInformation.Create);
        RuleFor(p=>p.Height).MustBeValueObject(PetHeight.Create);
        RuleFor(p=>p.Weight).MustBeValueObject(PetWeight.Create);
        RuleFor(p=>p.Birthday).MustBeValueObject(Birthday.Create);
        RuleFor(p=>p.OwnersPhoneNumber).MustBeValueObject(OwnersPhoneNumber.Create);
        RuleFor(p=>p.Description).MustBeValueObject(PetDescription.Create);
    }
}