using PetFamily.Discussions.Application.Discussions.Command.Create;

namespace PetFamily.Discussions.Presentation.Requests;

public record CreateDiscussionRequest(Guid RelationId, IEnumerable<Guid> UsersIds)
{
    public CreateDiscussionCommand ToCommand() =>
        new(RelationId, UsersIds);
}