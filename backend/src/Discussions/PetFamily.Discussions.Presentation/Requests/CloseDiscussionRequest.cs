using PetFamily.Discussions.Application.Discussions.Command.Close;

namespace PetFamily.Discussions.Presentation.Requests;

public record CloseDiscussionRequest(Guid DiscussionId, Guid AdminId)
{
    public CloseDiscussionCommand ToCommand()
        => new(DiscussionId, AdminId);
}