using PetFamily.Core.Abstractions;

namespace PetFamily.VolunteersApplications.Application.VolunteersApplications.Commands.Approve;

public record ApproveApplicationCommand(Guid ApplicationId, Guid AdminId) : ICommand;