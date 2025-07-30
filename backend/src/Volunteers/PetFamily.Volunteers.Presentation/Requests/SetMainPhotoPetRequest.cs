using PetFamily.Volunteers.Application.Volunteers.Commands.SetMainPhotoPet;

namespace PetFamily.Volunteers.Presentation.Requests;

public record SetMainPhotoPetRequest(string PhotoPath)
{
    public SetMainPhotoPetCommand ToCommand(Guid volunteerId, Guid petId)
        => new(volunteerId, petId, PhotoPath);
}