using PetFamily.Application.Volunteers.Queries.GetVolunteersWithPagination;

namespace PetFamily.API.Controllers.Volunteers.Requests;

public record GetVolunteerWithPaginationRequest(int Page, int PageSize)
{
    public GetVolunteersWithPaginationQuery ToQuery()
        => new(Page, PageSize);
}