using PetFamily.Core.Abstractions;

namespace PetFamily.Discussions.Application.Discussions.Command.EditMessage;

public record EditMessageCommand(string Message, Guid UserId, Guid MessageId,Guid DiscussionId) : ICommand;