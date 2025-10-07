using CSharpFunctionalExtensions;
using PetFamily.Discussions.Domain.DiscussionManagement;
using PetFamily.SharedKernel;

namespace PetFamily.Discussions.Application.Discussions;

public interface IDiscussionRepository
{
    public Task<Guid> AddDiscussion(Discussion discussion, CancellationToken cancellationToken);
    public Task<Result<Discussion, Error>> GetById(Guid id, CancellationToken cancellationToken);
    public Task Save(
        Discussion discussion,
        CancellationToken cancellationToken = default);
}