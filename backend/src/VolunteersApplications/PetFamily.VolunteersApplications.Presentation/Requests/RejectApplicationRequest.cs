using PetFamily.VolunteersApplications.Application.VolunteersApplications.Commands.Reject;

namespace PetFamily.VolunteersApplications.Presentation.Requests;

public record RejectApplicationRequest(string Comment, Guid ApplicationId, Guid AdminId)
{
    public RejectApplicationCommand ToCommand()
        => new(Comment, ApplicationId, AdminId);
}