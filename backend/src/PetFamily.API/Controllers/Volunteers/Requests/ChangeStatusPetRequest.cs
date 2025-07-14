using PetFamily.Application.Volunteers.Commands.ChangeStatusPet;

namespace PetFamily.API.Controllers.Volunteers.Requests;

public record ChangeStatusPetRequest(int Status)
{
    public ChangeStatusPetCommand ToCommand(Guid volunteerId, Guid petId)
        => new(volunteerId, petId, Status);
}