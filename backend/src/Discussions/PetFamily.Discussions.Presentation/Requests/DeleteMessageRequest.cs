using PetFamily.Discussions.Application.Discussions.Command.DeleteMessage;

namespace PetFamily.Discussions.Presentation.Requests;

public record DeleteMessageRequest(Guid MessageId, Guid UserId, Guid DiscussionId)
{
    public DeleteMessageCommand ToCommand()
        => new(MessageId, UserId, DiscussionId);
}