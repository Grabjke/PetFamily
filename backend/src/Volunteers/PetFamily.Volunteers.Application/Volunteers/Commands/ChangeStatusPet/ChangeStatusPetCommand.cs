using PetFamily.Core.Abstractions;

namespace PetFamily.Volunteers.Application.Volunteers.Commands.ChangeStatusPet;

public record ChangeStatusPetCommand(Guid VolunteerId, Guid PetId, int Status) : ICommand;