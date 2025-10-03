using PetFamily.Core.Abstractions;

namespace PetFamily.Discussions.Application.Discussions.Command.Create;

public record CreateDiscussionCommand(Guid RelationId, IEnumerable<Guid> UsersIds) : ICommand;