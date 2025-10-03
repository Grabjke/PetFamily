using PetFamily.VolunteersApplications.Application.VolunteersApplications.Queries.GetApplicationsByUserIdWithPagination;

namespace PetFamily.VolunteersApplications.Presentation.Requests;

public record GetApplicationsByUserIdWithPaginationRequest(
    int Page,
    int PageSize,
    int? Status)
{
    public GetApplicationsByUserIdWithPaginationQuery ToQuery(Guid UserId) =>
        new(Page, PageSize, UserId, Status);
}