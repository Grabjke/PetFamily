using Microsoft.EntityFrameworkCore;
using PetFamily.Core;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Dtos.Query;

namespace PetFamily.Discussions.Application.Discussions.Queries.GetByRelationId;

public class GetByRelationIdHandler : IQueryHandler<DiscussionDto, GetByRelationIdQuery>
{
    private readonly IDiscussionReadDbContext _context;

    public GetByRelationIdHandler(IDiscussionReadDbContext context)
    {
        _context = context;
    }

    public async Task<DiscussionDto> Handle(GetByRelationIdQuery query, CancellationToken cancellationToken = default)
    {
        var discussion = await _context.Discussions
            .Include(d => d.Messages)
            .FirstOrDefaultAsync(d => d.RelationId == query.RelationId, cancellationToken);

        return discussion ?? new DiscussionDto();
    }
}