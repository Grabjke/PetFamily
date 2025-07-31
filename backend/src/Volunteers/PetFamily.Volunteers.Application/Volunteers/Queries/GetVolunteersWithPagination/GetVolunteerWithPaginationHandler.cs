using PetFamily.Core;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Dtos.Query;
using PetFamily.Core.Extensions;
using PetFamily.Core.Models;

namespace PetFamily.Volunteers.Application.Volunteers.Queries.GetVolunteersWithPagination;

public class GetVolunteerWithPaginationHandler :
    IQueryHandler<PagedList<VolunteerDto>, GetVolunteersWithPaginationQuery>
{
    private readonly IVolunteersReadDbContext _readDbContext;

    public GetVolunteerWithPaginationHandler(IVolunteersReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }

    public async Task<PagedList<VolunteerDto>> Handle(
        GetVolunteersWithPaginationQuery query,
        CancellationToken cancellationToken)
    {
        var volunteerQuery = _readDbContext.Volunteers;

        return await volunteerQuery.ToPagedList(query.Page, query.PageSize, cancellationToken);
    }
}