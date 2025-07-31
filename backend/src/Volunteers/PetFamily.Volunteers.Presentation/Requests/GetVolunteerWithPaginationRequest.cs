using PetFamily.Volunteers.Application.Volunteers.Queries.GetVolunteersWithPagination;

namespace PetFamily.Volunteers.Presentation.Requests;

public record GetVolunteerWithPaginationRequest(int Page, int PageSize)
{
    public GetVolunteersWithPaginationQuery ToQuery()
        => new(Page, PageSize);
}