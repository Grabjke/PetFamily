using PetFamily.Core.Abstractions;

namespace PetFamily.Volunteers.Application.Volunteers.Commands.MovePetPosition;

public record MovePetPositionCommand(Guid VolunteerId, Guid PetId, int Position) : ICommand;