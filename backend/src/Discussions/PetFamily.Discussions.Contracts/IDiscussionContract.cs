using CSharpFunctionalExtensions;
using PetFamily.Discussions.Contracts.Requests;
using PetFamily.SharedKernel;

namespace PetFamily.Discussions.Contracts;

public interface IDiscussionContract
{
    Task<Result<Guid,ErrorList>> AddDiscussion(
        AddDiscussionRequest request,
        CancellationToken cancellationToken);
}