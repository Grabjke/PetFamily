using PetFamily.Application.Abstractions;

namespace PetFamily.Application.Volunteers.Command.MovePetPosition;

public record MovePetPositionCommand(Guid VolunteerId, Guid PetId, int Position):ICommand;
