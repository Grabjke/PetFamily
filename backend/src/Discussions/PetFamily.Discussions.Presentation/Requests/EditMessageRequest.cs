using PetFamily.Discussions.Application.Discussions.Command.EditMessage;

namespace PetFamily.Discussions.Presentation.Requests;

public record EditMessageRequest(string Message, Guid UserId, Guid MessageId, Guid DiscussionId)
{
    public EditMessageCommand ToCommand()
        => new(Message, UserId, MessageId, DiscussionId);
}