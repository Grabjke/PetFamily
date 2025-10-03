using CSharpFunctionalExtensions;
using PetFamily.Core.ValueObjects.VolunteerApplication;
using PetFamily.SharedKernel;
using PetFamily.VolunteersApplications.Domain.Enums;

namespace PetFamily.VolunteersApplications.Domain.ApplicationManagement;

public class VolunteerApplication : SharedKernel.Entity<Guid>
{
    public Guid UserId { get; private set; }
    public Guid? AdminId { get; private set; }
    public Guid? DiscussionId { get; private set; }
    public VolunteerInfo VolunteerInfo { get; private set; }
    public VolunteerRequestStatus Status { get; private set; }
    public string? Comment { get; private set; }
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

    public UnitResult<Error> Approve(Guid adminId)
    {
        if (adminId != AdminId)
            return Errors.General.NotFound();

        Status = VolunteerRequestStatus.Approved;
        UpdatedAt = DateTime.UtcNow;

        return Result.Success<Error>();
    }

    public UnitResult<Error> Reject(Guid adminId, string comment)
    {
        if (adminId != AdminId)
            return Errors.General.NotFound();;

        Status = VolunteerRequestStatus.Rejected;
        Comment = comment;
        UpdatedAt = DateTime.UtcNow;

        return Result.Success<Error>();
    }

    public UnitResult<Error> Revision(Guid adminId, string comment)
    {
        if (adminId != AdminId)
            return Errors.General.NotFound();

        Status = VolunteerRequestStatus.Revision;
        Comment = comment;
        UpdatedAt = DateTime.UtcNow;

        return Result.Success<Error>();
    }
    public UnitResult<Error> Update(Guid userId,VolunteerInfo information)
    {
        if(userId != UserId)
            return Errors.General.NotFound();
        
        VolunteerInfo = information;
        
        return Result.Success<Error>();
    }
}