using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.Discussions.Application.Discussions;
using PetFamily.Discussions.Domain.DiscussionManagement;
using PetFamily.Discussions.Infrastructure.DbContexts;
using PetFamily.SharedKernel;

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

    public async Task<Result<Discussion, Error>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var discussion = await _context.Discussions
            .Include(d=>d.Messages)
            .FirstOrDefaultAsync(d => d.Id == id, cancellationToken);

        if (discussion is null)
            return Errors.General.NotFound();

        return discussion;
    }

    public async Task Save(Discussion discussion, CancellationToken cancellationToken = default)
    {
        _context.Discussions.Update(discussion);
        await _context.SaveChangesAsync(cancellationToken);
    }
}