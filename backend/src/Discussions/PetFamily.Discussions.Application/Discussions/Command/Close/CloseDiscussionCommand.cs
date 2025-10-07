using PetFamily.Core.Abstractions;

namespace PetFamily.Discussions.Application.Discussions.Command.Close;

public record CloseDiscussionCommand(Guid DiscussionId,Guid AdminId) : ICommand;