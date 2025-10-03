using PetFamily.Core.Abstractions;

namespace PetFamily.VolunteersApplications.Application.VolunteersApplications.Commands.Revision;

public record RevisionApplicationCommand(
    string Comment,
    Guid ApplicationId,
    Guid AdminId) : ICommand;