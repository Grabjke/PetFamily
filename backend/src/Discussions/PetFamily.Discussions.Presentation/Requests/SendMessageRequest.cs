using PetFamily.Discussions.Application.Discussions.Command.SendMessage;

namespace PetFamily.Discussions.Presentation.Requests;

public record SendMessageRequest(Guid DiscussionId, string Message, Guid UserId)
{
    public SendMessageCommand ToCommand()
        => new(DiscussionId, Message, UserId);
}