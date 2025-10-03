using PetFamily.Core.Abstractions;

namespace PetFamily.VolunteersApplications.Application.VolunteersApplications.Queries.
    GetApplicationsNotAcceptedWithPagination;

public record GetApplicationsNotAcceptedWithPaginationQuery(int Page, int PageSize) : IQuery;