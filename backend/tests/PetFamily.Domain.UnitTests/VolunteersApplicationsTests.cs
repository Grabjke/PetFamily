using FluentAssertions;
using PetFamily.Core.ValueObjects.VolunteerApplication;
using PetFamily.VolunteersApplications.Domain.ApplicationManagement;
using PetFamily.VolunteersApplications.Domain.Enums;

namespace PetFamily.UnitTests;

public class VolunteersApplicationsTests
{
    [Fact]
    public void Create_ShouldReturnSuccess_WhenDataIsValid()
    {
        var userId = Guid.NewGuid();
        var volunteerInfoResult = VolunteerInfo.Create(
            "firstName",
            "lastName",
            "79920013488",
            "adasdasASB12@gmail.com",
            "surname",
            "description");

        volunteerInfoResult.IsSuccess.Should().BeTrue();
        var volunteerInfo = volunteerInfoResult.Value;

        var result = VolunteerApplication.Create(userId, volunteerInfo);

        result.IsSuccess.Should().BeTrue();
        result.Value.VolunteerInfo.Should().Be(volunteerInfo);
        result.Value.UserId.Should().Be(userId);
    }

    [Fact]
    public void Create_ShouldReturnFailure_WhenRequiredFieldsInvalid()
    {
        var userId = Guid.NewGuid();

        var volunteerInfoResult = VolunteerInfo.Create(
            "",
            "lastName",
            "+79920013488",
            "adasdasASB12@gmail.com",
            "surname",
            "description");

        volunteerInfoResult.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public void Take_to_review_should_success()
    {
        var userId = Guid.NewGuid();
        var adminId = Guid.NewGuid();
        var discussionId = Guid.NewGuid();
        var volunteerInfoResult = VolunteerInfo.Create(
            "firstName",
            "lastName",
            "79920013488",
            "adasdasASB12@gmail.com",
            "surname",
            "description");
        volunteerInfoResult.IsSuccess.Should().BeTrue();
        var volunteerInfo = volunteerInfoResult.Value;
        var applicationResult = VolunteerApplication.Create(userId, volunteerInfo);
        applicationResult.IsSuccess.Should().BeTrue();
        var volunteerApplication = applicationResult.Value;

        volunteerApplication.TakeToReview(adminId, discussionId);

        volunteerApplication.Status.Should().Be(VolunteerRequestStatus.OnReview);
        volunteerApplication.AdminId.Should().Be(adminId);
        volunteerApplication.DiscussionId.Should().Be(discussionId);
    }

    [Fact]
    public void Approve_ShouldSetStatusToApproved()
    {
        var userId = Guid.NewGuid();
        var adminId = Guid.NewGuid();
        var volunteerInfoResult = VolunteerInfo.Create(
            "ValidFirstName",
            "lastName",
            "79920013488",
            "adasdasASB12@gmail.com",
            "surname",
            "description");
        volunteerInfoResult.IsSuccess.Should().BeTrue();
        var volunteerInfo = volunteerInfoResult.Value;
        var applicationResult = VolunteerApplication.Create(userId, volunteerInfo);
        applicationResult.IsSuccess.Should().BeTrue();
        var volunteerApplication = applicationResult.Value;

        volunteerApplication.Approve(adminId);

        volunteerApplication.Status.Should().Be(VolunteerRequestStatus.Approved);
        volunteerApplication.AdminId.Should().Be(adminId);
    }

    [Fact]
    public void Reject_ShouldSetStatusToRejectedAndStoreComment()
    {
        var comment = "Rejection comment";
        var userId = Guid.NewGuid();
        var adminId = Guid.NewGuid();
        var volunteerInfoResult = VolunteerInfo.Create(
            "ValidFirstName",
            "lastName",
            "79920013488",
            "adasdasASB12@gmail.com",
            "surname",
            "description");
        volunteerInfoResult.IsSuccess.Should().BeTrue();
        var volunteerInfo = volunteerInfoResult.Value;
        var applicationResult = VolunteerApplication.Create(userId, volunteerInfo);
        applicationResult.IsSuccess.Should().BeTrue();
        var volunteerApplication = applicationResult.Value;

        volunteerApplication.Reject(adminId, comment);

        volunteerApplication.Status.Should().Be(VolunteerRequestStatus.Rejected);
        volunteerApplication.Comment.Should().Be(comment);
        volunteerApplication.AdminId.Should().Be(adminId);
    }

    [Fact]
    public void Create_ShouldReturnFailure_WhenUserIdIsEmpty()
    {
        var emptyUserId = Guid.Empty;
        var volunteerInfoResult = VolunteerInfo.Create(
            "firstName",
            "lastName",
            "79920013488",
            "adasdasASB12@gmail.com",
            "surname",
            "description");
        volunteerInfoResult.IsSuccess.Should().BeTrue();
        var volunteerInfo = volunteerInfoResult.Value;

        var result = VolunteerApplication.Create(emptyUserId, volunteerInfo);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().NotBeNull();
    }
}