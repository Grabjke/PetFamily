using PetFamily.Core.Abstractions;

namespace PetFamily.Discussions.Application.Discussions.Command.SendMessage;

public record SendMessageCommand(Guid DiscussionId, string Message, Guid UserId) : ICommand;