using PetFamily.Core.Abstractions;

namespace PetFamily.Discussions.Application.Discussions.Command.DeleteMessage;

public record DeleteMessageCommand(Guid MessageId, Guid UserId, Guid DiscussionId) : ICommand;