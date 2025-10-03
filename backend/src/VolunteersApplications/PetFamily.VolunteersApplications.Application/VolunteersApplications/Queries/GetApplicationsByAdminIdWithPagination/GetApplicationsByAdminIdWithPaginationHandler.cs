using PetFamily.Core;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Dtos.Query;
using PetFamily.Core.Extensions;
using PetFamily.Core.Models;

namespace PetFamily.VolunteersApplications.Application.VolunteersApplications.Queries.
    GetApplicationsByAdminIdWithPagination;

public class GetApplicationsByAdminIdWithPaginationHandler :
    IQueryHandler<PagedList<ApplicationDto>, GetApplicationsByAdminIdWithPaginationQuery>
{
    private readonly IApplicationReadDbContext _context;

    public GetApplicationsByAdminIdWithPaginationHandler(IApplicationReadDbContext context)
    {
        _context = context;
    }

    public async Task<PagedList<ApplicationDto>> Handle(
        GetApplicationsByAdminIdWithPaginationQuery query,
        CancellationToken cancellationToken = default)
    {
        var applications = _context.Applications
            .Where(a => a.AdminId == query.AdminId);

        applications = query.Status.HasValue 
            ? applications.Where(a => a.Status == (VolunteerRequestStatusDto)query.Status.Value) 
            : applications.Where(a => a.Status == VolunteerRequestStatusDto.OnReview);

        return await applications.ToPagedList(query.Page, query.PageSize, cancellationToken);
    }
}