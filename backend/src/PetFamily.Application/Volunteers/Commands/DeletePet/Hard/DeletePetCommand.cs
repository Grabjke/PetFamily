using PetFamily.Application.Abstractions;

namespace PetFamily.Application.Volunteers.Commands.DeletePet.Hard;

public record DeletePetCommand(Guid VolunteerId, Guid PetId):ICommand;