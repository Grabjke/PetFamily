
using PetFamily.Core.Abstractions;
using PetFamily.Core.Dtos;

namespace PetFamily.VolunteersApplications.Application.VolunteersApplications.Commands.Create;

public record CreateApplicationCommand(Guid UserId,VolunteerInfoDto VolunteerInfo) : ICommand;
