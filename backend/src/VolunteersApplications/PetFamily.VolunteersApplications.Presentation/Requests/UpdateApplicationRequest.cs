using PetFamily.Core.Dtos;
using PetFamily.VolunteersApplications.Application.VolunteersApplications.Commands.Update;

namespace PetFamily.VolunteersApplications.Presentation.Requests;

public record UpdateApplicationRequest(Guid ApplicationId, Guid UserId, VolunteerInfoDto VolunteerInfo)
{
    public UpdateApplicationCommand ToCommand()
        => new UpdateApplicationCommand(ApplicationId, UserId, VolunteerInfo);
}