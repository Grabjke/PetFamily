using PetFamily.VolunteersApplications.Application.VolunteersApplications.Commands.Approve;

namespace PetFamily.VolunteersApplications.Presentation.Requests;

public record ApproveApplicationRequest(Guid ApplicationId, Guid AdminId)
{
    public ApproveApplicationCommand ToCommand() => new(ApplicationId, AdminId);
}