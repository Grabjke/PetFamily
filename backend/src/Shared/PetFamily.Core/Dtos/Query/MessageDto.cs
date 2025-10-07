namespace PetFamily.Core.Dtos.Query;
public class MessageDto
{
    public Guid Id { get; init; }
    public string Text { get; init; }=string.Empty;
    public DateTime CreatedAt { get; init; }
    public bool IsEdited { get; init; }
    public Guid UserId { get; init; }
    public Guid DiscussionId { get; init; }
}