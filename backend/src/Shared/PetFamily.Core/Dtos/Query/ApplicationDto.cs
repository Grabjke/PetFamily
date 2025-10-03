namespace PetFamily.Core.Dtos.Query;

public class ApplicationDto
{
    public Guid Id { get; init; }
    public Guid UserId { get; init; }
    public Guid? AdminId { get; init; }
    public Guid? DiscussionId { get; init; }
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string? Surname { get; init; }
    public string PhoneNumber { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string? Description { get; init; }
    public VolunteerRequestStatusDto Status { get; init; }
    public string? Comment { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
}