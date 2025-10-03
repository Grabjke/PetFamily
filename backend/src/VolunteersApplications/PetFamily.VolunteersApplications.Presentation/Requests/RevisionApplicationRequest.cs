using PetFamily.VolunteersApplications.Application.VolunteersApplications.Commands.Revision;

namespace PetFamily.VolunteersApplications.Presentation.Requests;

public record RevisionApplicationRequest(string Comment, Guid ApplicationId, Guid AdminId)
{
    public RevisionApplicationCommand ToCommand()
        => new(Comment, ApplicationId, AdminId);
}