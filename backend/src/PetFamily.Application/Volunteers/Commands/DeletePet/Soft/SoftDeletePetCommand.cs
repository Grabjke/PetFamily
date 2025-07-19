using PetFamily.Application.Abstractions;

namespace PetFamily.Application.Volunteers.Commands.DeletePet.Soft;

public record SoftDeletePetCommand(Guid VolunteerId, Guid PetId):ICommand;
