namespace PetFamily.Discussions.Contracts.Requests;

public record AddDiscussionRequest(Guid RelationId, IEnumerable<Guid> UsersIds);