using PetFamily.Core.Abstractions;

namespace PetFamily.VolunteersApplications.Application.VolunteersApplications.Commands.Take;

public record TakeApplicationCommand(Guid ApplicationId, Guid AdminId) : ICommand;
