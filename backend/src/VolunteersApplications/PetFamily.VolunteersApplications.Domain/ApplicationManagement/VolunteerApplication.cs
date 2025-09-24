using CSharpFunctionalExtensions;
using PetFamily.Core.ValueObjects.VolunteerApplication;
using PetFamily.SharedKernel;
using PetFamily.VolunteersApplications.Domain.Enums;

namespace PetFamily.VolunteersApplications.Domain.ApplicationManagement;

public class VolunteerApplication : SharedKernel.Entity<Guid>
{
    public new Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public Guid? AdminId { get; private set; }
    public Guid? DiscussionId { get; private set; }
    public VolunteerInfo VolunteerInfo { get; private set; }
    public VolunteerRequestStatus Status { get; private set; }
    public string? RejectionComment { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    //ef
    private VolunteerApplication(Guid id) : base(id)
    {
    }

    private VolunteerApplication(
        Guid id,
        Guid userId,
        VolunteerInfo volunteerInfo) : base(id)
    {
        UserId = userId;
        VolunteerInfo = volunteerInfo;
        Status = VolunteerRequestStatus.Created;
        CreatedAt = DateTime.UtcNow;
    }

    public static Result<VolunteerApplication, Error> Create(Guid userId, VolunteerInfo volunteerInfo)
    {
        if (userId == Guid.Empty)
            return Errors.General.ValueIsInvalid("UserId");

        var applicationId = Guid.NewGuid();

        return new VolunteerApplication(
            applicationId,
            userId,
            volunteerInfo);
    }

    public void TakeToReview(Guid adminId, Guid discussionId)
    {
        AdminId = adminId;
        DiscussionId = discussionId;
        Status = VolunteerRequestStatus.OnReview;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Approve(Guid adminId)
    {
        AdminId = adminId;
        Status = VolunteerRequestStatus.Approved;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Reject(Guid adminId, string comment)
    {
        AdminId = adminId;
        Status = VolunteerRequestStatus.Rejected;
        RejectionComment = comment;
        UpdatedAt = DateTime.UtcNow;
    }
}