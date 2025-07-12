using PetFamily.Application.Abstractions;

namespace PetFamily.Application.Volunteers.Commands.MovePetPosition;

public record MovePetPositionCommand(Guid VolunteerId, Guid PetId, int Position):ICommand;
