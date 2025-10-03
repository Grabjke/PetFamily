using PetFamily.Core.Abstractions;

namespace PetFamily.VolunteersApplications.Application.VolunteersApplications.Commands.Reject;

public record RejectApplicationCommand(
    string Comment,
    Guid ApplicationId,
    Guid AdminId) : ICommand;