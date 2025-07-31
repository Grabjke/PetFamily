using PetFamily.Core.Abstractions;

namespace PetFamily.Volunteers.Application.Volunteers.Commands.DeletePet.Soft;

public record SoftDeletePetCommand(Guid VolunteerId, Guid PetId) : ICommand;