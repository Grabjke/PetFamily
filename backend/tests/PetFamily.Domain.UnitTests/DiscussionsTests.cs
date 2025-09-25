using FluentAssertions;
using PetFamily.Core.ValueObjects.Discussion;
using PetFamily.Discussions.Domain.DiscussionManagement;

namespace PetFamily.UnitTests;

public class DiscussionsTests
{
    [Fact]
    public void Success_create_discussion()
    {
        var relationId = Guid.NewGuid();
        var firstUserId = Guid.NewGuid();
        var secondUserId = Guid.NewGuid();

        var result = Discussion.Create(relationId, [firstUserId, secondUserId]);

        result.IsSuccess.Should().BeTrue();
        result.Value.UsersIds.Should().HaveCount(2);
    }

    [Fact]
    public void Success_add_message()
    {
        var relationId = Guid.NewGuid();
        var firstUserId = Guid.NewGuid();
        var secondUserId = Guid.NewGuid();
        var text = Text.Create("Text").Value;
        var discussion = Discussion.Create(relationId, [firstUserId, secondUserId]).Value;

        var result = discussion.AddMessage(text, firstUserId);

        result.IsSuccess.Should().BeTrue();
        discussion.Messages.Should().HaveCount(1);
    }

    [Fact]
    public void Success_remove_message()
    {
        var relationId = Guid.NewGuid();
        var firstUserId = Guid.NewGuid();
        var secondUserId = Guid.NewGuid();
        var text = Text.Create("Text").Value;
        var discussion = Discussion.Create(relationId, [firstUserId, secondUserId]).Value;
        var messageId = discussion.AddMessage(text, firstUserId).Value;

        var result = discussion.RemoveMessage(messageId, firstUserId);

        result.IsSuccess.Should().BeTrue();
        discussion.Messages.Should().HaveCount(0);
    }

    [Fact]
    public void Success_edit_message()
    {
        var relationId = Guid.NewGuid();
        var firstUserId = Guid.NewGuid();
        var secondUserId = Guid.NewGuid();
        var text = Text.Create("Text").Value;
        var newText = Text.Create("New Text").Value;
        var discussion = Discussion.Create(relationId, [firstUserId, secondUserId]).Value;
        var messageId = discussion.AddMessage(text, firstUserId).Value;

        var result = discussion.EditMessage(newText, messageId, firstUserId);

        result.IsSuccess.Should().BeTrue();
        discussion.Messages.Should().HaveCount(1);
        discussion.Messages[0].Text.Should().Be(newText);
    }
    [Fact]
    public void Failure_edit_message_because_wrong_user_id()
    {
        var relationId = Guid.NewGuid();
        var firstUserId = Guid.NewGuid();
        var secondUserId = Guid.NewGuid();
        var text = Text.Create("Text").Value;
        var newText = Text.Create("New Text").Value;
        var discussion = Discussion.Create(relationId, [firstUserId, secondUserId]).Value;
        var messageId = discussion.AddMessage(text, firstUserId).Value;

        var result = discussion.EditMessage(newText, messageId, secondUserId);

        result.IsSuccess.Should().BeFalse();
        discussion.Messages.Should().HaveCount(1);
        discussion.Messages[0].Text.Should().Be(text);
    }
}