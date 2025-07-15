using PetFamily.Application.Dtos;
using PetFamily.Application.Volunteers.Commands.UpdateMainInfoPet;
using PetFamily.Domain.ValueObjects.Pet;

namespace PetFamily.API.Controllers.Volunteers.Requests;

public record UpdateMainInfoPetRequest(
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
    public UpdateMainInfoPetCommand ToCommand(Guid volunteerId, Guid petId)
        => new(
            volunteerId,
            petId,
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