
using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.SharedKernel;
using PetFamily.Volunteers.Domain.PetManagement.ValueObjects.Pet;
using PetFamily.Volunteers.Domain.PetManagement.ValueObjects.Volunteer;

namespace PetFamily.Volunteers.Application.Volunteers.Commands.AddPet;

public class AddPetCommandValidator : AbstractValidator<AddPetCommand>
{
    public AddPetCommandValidator()
    {
        RuleFor(p => p.VolunteerId)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired());

        RuleFor(p => p.SpeciesId)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired());

        RuleFor(p => p.BreedId)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired());

        RuleFor(p => p.Name).MustBeValueObject(PetName.Create);
        RuleFor(p => p.Address)
            .MustBeValueObject(c =>
                Address.Create(c.Street, c.City, c.Country, c.ZipCode));
        RuleFor(p => p.Colour).MustBeValueObject(Colour.Create);
        RuleFor(p => p.HealthInformation).MustBeValueObject(HealthInformation.Create);
        RuleFor(p => p.Height).MustBeValueObject(Height.Create);
        RuleFor(p => p.Weight).MustBeValueObject(Weight.Create);
        RuleFor(p => p.Birthday).MustBeValueObject(Birthday.Create);
        RuleFor(p => p.OwnersPhoneNumber).MustBeValueObject(OwnersPhoneNumber.Create);
        RuleFor(p => p.Description).MustBeValueObject(Description.Create);
    }
}