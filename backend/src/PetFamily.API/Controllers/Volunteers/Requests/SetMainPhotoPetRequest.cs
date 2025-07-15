using PetFamily.Application.Volunteers.Commands.SetMainPhotoPet;

namespace PetFamily.API.Controllers.Volunteers.Requests;

public record SetMainPhotoPetRequest(string PhotoPath)
{
    public SetMainPhotoPetCommand ToCommand(Guid volunteerId, Guid petId)
        => new(volunteerId, petId, PhotoPath);
}