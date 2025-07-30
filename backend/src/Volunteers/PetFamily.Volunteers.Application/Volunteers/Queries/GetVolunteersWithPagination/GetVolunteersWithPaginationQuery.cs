using PetFamily.Core.Abstractions;

namespace PetFamily.Volunteers.Application.Volunteers.Queries.GetVolunteersWithPagination;

public record GetVolunteersWithPaginationQuery(int Page, int PageSize) : IQuery;