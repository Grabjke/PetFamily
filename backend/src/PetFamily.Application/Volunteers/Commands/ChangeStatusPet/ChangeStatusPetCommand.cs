using PetFamily.Application.Abstractions;

namespace PetFamily.Application.Volunteers.Commands.ChangeStatusPet;

public record ChangeStatusPetCommand(Guid VolunteerId, Guid PetId, int Status) : ICommand;