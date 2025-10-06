using FluentAssertions;
using PetFamily.Core.ValueObjects.VolunteerApplication;
using PetFamily.VolunteersApplications.Domain.ApplicationManagement;
using PetFamily.VolunteersApplications.Domain.Enums;

namespace PetFamily.UnitTests;

public class VolunteersTests
{
    [Fact]
    public void Volunteer_application_should_be_created_when_data_is_valid()
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
    public void Volunteer_application_creation_should_fail_when_required_fields_are_invalid()
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
    public void Volunteer_application_should_be_taken_to_review_when_admin_and_discussion_are_provided()
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
    public void Volunteer_application_status_should_be_approved_when_admin_approves()
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
    public void Volunteer_application_status_should_be_rejected_with_comment_when_admin_rejects()
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
    public void Volunteer_application_creation_should_fail_when_user_id_is_empty()
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