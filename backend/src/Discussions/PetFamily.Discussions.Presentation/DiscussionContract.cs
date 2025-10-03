using CSharpFunctionalExtensions;
using PetFamily.Discussions.Application.Discussions.Command.Create;
using PetFamily.Discussions.Contracts;
using PetFamily.Discussions.Contracts.Requests;
using PetFamily.SharedKernel;

namespace PetFamily.Discussions.Presentation;

public class DiscussionContract : IDiscussionContract
{
    private readonly CreateDiscussionHandler _handler;

    public DiscussionContract(CreateDiscussionHandler handler)
    {
        _handler = handler;
    }

    public async Task<Result<Guid, ErrorList>> AddDiscussion(
        AddDiscussionRequest request, CancellationToken cancellationToken)
    {
        var discussionResult = await _handler.Handle(
            new CreateDiscussionCommand(request.RelationId, request.UsersIds), cancellationToken);
        if (discussionResult.IsSuccess == false)
            return discussionResult.Error;

        return discussionResult;
    }
}