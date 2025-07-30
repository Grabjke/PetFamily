using PetFamily.Volunteers.Application.Volunteers.Commands.ChangeStatusPet;

namespace PetFamily.Volunteers.Presentation.Requests;

public record ChangeStatusPetRequest(int Status)
{
    public ChangeStatusPetCommand ToCommand(Guid volunteerId, Guid petId)
        => new(volunteerId, petId, Status);
}