using PetFamily.Discussions.Application.Discussions;
using PetFamily.Discussions.Domain.DiscussionManagement;
using PetFamily.Discussions.Infrastructure.DbContexts;

namespace PetFamily.Discussions.Infrastructure.Repositories;

public class DiscussionRepository : IDiscussionRepository
{
    private readonly WriteDiscussionDbContext _context;

    public DiscussionRepository(WriteDiscussionDbContext context)
    {
        _context = context;
    }


    public async Task<Guid> AddDiscussion(Discussion discussion, CancellationToken cancellationToken)
    {
        await _context.AddAsync(discussion, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return discussion.Id;
    }
}