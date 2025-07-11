using Microsoft.EntityFrameworkCore;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.Dtos.Query;
using PetFamily.Application.Extensions;
using PetFamily.Application.Models;

namespace PetFamily.Application.Volunteers.Queries.GetVolunteersWithPagination;

public class GetVolunteerWithPaginationHandler :
    IQueryHandler<PagedList<VolunteerDto>, GetVolunteersWithPaginationQuery>
{
    private readonly IReadDbContext _readDbContext;

    public GetVolunteerWithPaginationHandler(IReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }

    public async Task<PagedList<VolunteerDto>> Handle(
        GetVolunteersWithPaginationQuery query,
        CancellationToken cancellationToken)
    {
        var volunteerQuery = _readDbContext.Volunteers;

        //фильтрация

        return await volunteerQuery.ToPagedList(query.Page, query.PageSize, cancellationToken);
    }
}