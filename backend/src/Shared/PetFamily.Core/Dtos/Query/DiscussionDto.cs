namespace PetFamily.Core.Dtos.Query;

public class DiscussionDto
{
    public Guid Id { get; init; }
    public IReadOnlyList<Guid> UsersIds { get; init; } = [];
    public DiscussionStatusDto Status { get; init; }
    public Guid RelationId { get; init; }
    public List<MessageDto> Messages { get; init; } = [];
}

public enum DiscussionStatusDto
{
    Open,
    Closed
}