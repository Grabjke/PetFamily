using PetFamily.Core.Abstractions;
using PetFamily.Core.Dtos;

namespace PetFamily.VolunteersApplications.Application.VolunteersApplications.Commands.Update;

public record UpdateApplicationCommand(Guid ApplicationId, Guid UserId, VolunteerInfoDto VolunteerInfo) : ICommand;