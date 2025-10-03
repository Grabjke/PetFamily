using PetFamily.VolunteersApplications.Application.VolunteersApplications.Queries.
    GetApplicationsNotAcceptedWithPagination;

namespace PetFamily.VolunteersApplications.Presentation.Requests;

public record GetApplicationsNotAcceptedWithPaginationRequest(int Page, int PageSize)
{
    public GetApplicationsNotAcceptedWithPaginationQuery ToQuery()
        => new(Page, PageSize);
}