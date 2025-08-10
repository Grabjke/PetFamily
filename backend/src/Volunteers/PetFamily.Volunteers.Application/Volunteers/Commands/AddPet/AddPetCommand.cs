using PetFamily.Core.Abstractions;
using PetFamily.Core.Dtos;
using PetFamily.Core.ValueObjects.Pet;

namespace PetFamily.Volunteers.Application.Volunteers.Commands.AddPet;

public record AddPetCommand(
    Guid VolunteerId,
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
    HelpStatus HelpStatus):ICommand;
    

