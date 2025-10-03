using PetFamily.Discussions.Domain.DiscussionManagement;

namespace PetFamily.Discussions.Application.Discussions;

public interface IDiscussionRepository
{
    public Task<Guid> AddDiscussion(Discussion discussion, CancellationToken cancellationToken);
}