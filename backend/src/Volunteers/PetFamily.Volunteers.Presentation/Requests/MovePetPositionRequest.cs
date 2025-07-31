using PetFamily.Volunteers.Application.Volunteers.Commands.MovePetPosition;

namespace PetFamily.Volunteers.Presentation.Requests;

public record MovePetPositionRequest(int Position)
{
    public MovePetPositionCommand ToCommand(Guid volunteerId,Guid petId) => new(
        volunteerId,
        petId,
        Position);
}