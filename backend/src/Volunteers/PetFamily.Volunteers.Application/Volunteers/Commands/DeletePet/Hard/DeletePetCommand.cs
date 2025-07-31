using PetFamily.Core.Abstractions;

namespace PetFamily.Volunteers.Application.Volunteers.Commands.DeletePet.Hard;

public record DeletePetCommand(Guid VolunteerId, Guid PetId) : ICommand;