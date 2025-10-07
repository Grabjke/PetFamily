using CSharpFunctionalExtensions;
using PetFamily.Core.ValueObjects.Discussion;
using PetFamily.Discussions.Domain.Enums;
using PetFamily.SharedKernel;

namespace PetFamily.Discussions.Domain.DiscussionManagement;

public class Discussion : SharedKernel.Entity<Guid>
{
    private const int MEMBERS_DISCUSSIONS_COUNT = 2;
    private readonly List<Guid> _usersIds = [];
    private readonly List<Message> _messages = [];

    //ef
    private Discussion(Guid id) : base(id)
    {
    }

    private Discussion(
        Guid id,
        Guid relationId,
        List<Guid> users) : base(id)
    {
        RelationId = relationId;
        _usersIds.AddRange(users);
        Status = DiscussionStatus.Open;
    }

    public Guid RelationId { get; private set; }
    public IReadOnlyList<Guid> UsersIds => _usersIds;
    public IReadOnlyList<Message> Messages => _messages;
    public DiscussionStatus Status { get; private set; }

    public static Result<Discussion, Error> Create(Guid relationId, IEnumerable<Guid> usersIds)
    {
        var usersList = usersIds.ToList();

        if (usersList.Count != MEMBERS_DISCUSSIONS_COUNT)
            return Errors.General.ValueIsInvalid("usersIds");

        if (relationId == Guid.Empty)
            return Errors.General.ValueIsInvalid("RelationId");

        var discussionId = Guid.NewGuid();

        return new Discussion(discussionId, relationId, usersList);
    }

    public void AddMessage(Message message)
    {
        _messages.Add(message);
    }

    public UnitResult<Error> RemoveMessage(
        Guid messageId,
        Guid userId)
    {
        var message = _messages.FirstOrDefault(m => m.Id == messageId && m.UserId == userId);
        if (message is null)
            return Errors.General.NotFound();

        _messages.Remove(message);

        return Result.Success<Error>();
    }


    public UnitResult<Error> EditMessage(
        Text text,
        Guid messageId,
        Guid userId)
    {
        var message = _messages.FirstOrDefault(m => m.Id == messageId && m.UserId == userId);
        if (message is null)
            return Errors.General.NotFound();

        message.EditText(text);

        return Result.Success<Error>();
    }

    public UnitResult<Error> CloseDiscussion(Guid adminId)
    {
        var adminExist = _usersIds.Contains(adminId);
        if (adminExist == false)
            return Errors.General.NotFound(adminId);

        Status = DiscussionStatus.Closed;

        return Result.Success<Error>();
    }
}