using CSharpFunctionalExtensions;
using PetFamily.Core.ValueObjects.Discussion;
using PetFamily.SharedKernel;

namespace PetFamily.Discussions.Domain.DiscussionManagement;

public class Message : SharedKernel.Entity<Guid>
{
    private Message(Guid id) : base(id)
    {
    }
    private Message(
        Guid id,
        Text text,
        Guid userId) : base(id)
    {
        Text = text;
        UserId = userId;
        IsEdited = false;
        CreatedAt = DateTime.Now;
    }
    public Text Text { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public bool IsEdited { get; private set; }
    public Guid UserId { get; private set; }
    
    public static Result<Message, Error> Create(Text text, Guid userId)
    {
        if (userId == Guid.Empty)
            return Errors.General.ValueIsInvalid("UserId");

        var messageId = Guid.NewGuid();

        return new Message(messageId, text, userId);
    }

    internal void EditText(Text text)
    {
        Text = text;
        IsEdited = true;
    }
    
    
}