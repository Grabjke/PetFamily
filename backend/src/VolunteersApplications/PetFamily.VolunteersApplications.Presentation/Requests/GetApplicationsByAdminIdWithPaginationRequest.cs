using
    PetFamily.VolunteersApplications.Application.VolunteersApplications.Queries.GetApplicationsByAdminIdWithPagination;

namespace PetFamily.VolunteersApplications.Presentation.Requests;

public record GetApplicationsByAdminIdWithPaginationRequest(
    int Page,
    int PageSize,
    int? Status)
{
    public GetApplicationsByAdminIdWithPaginationQuery ToQuery(Guid AdminId) =>
        new(Page, PageSize, AdminId, Status);
}