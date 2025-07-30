using PetFamily.Core.Dtos;
using PetFamily.Volunteers.Application.Volunteers.Commands.AddPet;
using PetFamily.Volunteers.Domain.PetManagement.ValueObjects.Pet;

namespace PetFamily.Volunteers.Presentation.Requests;

public record AddPetRequest(
    string Name,
    string Description,
    Guid SpeciesId,
    Guid BreedId,
    string Colour,
    string HealthInformation,
    AddressDto Address,
    double Weight,
    int Height,
    string OwnersPhoneNumber,
    bool Castration,
    DateTime Birthday,
    bool isVaccinated,
    HelpStatus HelpStatus)
{
    public AddPetCommand ToCommand(Guid volunteerId) => new(
        volunteerId,
        Name,
        Description,
        SpeciesId,
        BreedId,
        Colour,
        HealthInformation,
        Address,
        Weight,
        Height,
        OwnersPhoneNumber,
        Castration,
        Birthday,
        isVaccinated,
        HelpStatus);
}