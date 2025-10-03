using PetFamily.Core.Abstractions;

namespace PetFamily.VolunteersApplications.Application.VolunteersApplications.Queries.
    GetApplicationsByUserIdWithPagination;

public record GetApplicationsByUserIdWithPaginationQuery(
    int Page,
    int PageSize,
    Guid UserId,
    int? Status) : IQuery;