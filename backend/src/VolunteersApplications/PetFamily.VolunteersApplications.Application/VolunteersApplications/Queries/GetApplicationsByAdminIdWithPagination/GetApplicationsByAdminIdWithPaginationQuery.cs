using PetFamily.Core.Abstractions;

namespace PetFamily.VolunteersApplications.Application.VolunteersApplications.Queries.
    GetApplicationsByAdminIdWithPagination;

public record GetApplicationsByAdminIdWithPaginationQuery(
    int Page,
    int PageSize,
    Guid AdminId,
    int? Status) : IQuery;