using PetFamily.Core;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Dtos.Query;
using PetFamily.Core.Extensions;
using PetFamily.Core.Models;

namespace PetFamily.VolunteersApplications.Application.VolunteersApplications.Queries.
    GetApplicationsNotAcceptedWithPagination;

public class GetApplicationsNotAcceptedWithPaginationHandler :
    IQueryHandler<PagedList<ApplicationDto>, GetApplicationsNotAcceptedWithPaginationQuery>
{
    private readonly IApplicationReadDbContext _context;

    public GetApplicationsNotAcceptedWithPaginationHandler(IApplicationReadDbContext context)
    {
        _context = context;
    }

    public async Task<PagedList<ApplicationDto>> Handle(
        GetApplicationsNotAcceptedWithPaginationQuery query,
        CancellationToken cancellationToken = default)
    {
        var applications = _context.Applications
            .Where(a => a.Status == VolunteerRequestStatusDto.Created);

        return await applications.ToPagedList(query.Page, query.PageSize, cancellationToken);
    }
}