using PetFamily.VolunteersApplications.Application.VolunteersApplications.Commands.Take;

namespace PetFamily.VolunteersApplications.Presentation.Requests;

public record TakeApplicationRequest(Guid ApplicationId, Guid AdminId)
{
    public TakeApplicationCommand ToCommand() => new(ApplicationId, AdminId);
}