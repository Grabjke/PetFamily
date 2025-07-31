using PetFamily.Core.Abstractions;

namespace PetFamily.Species.Application.Species.Queries.GetSpeciesWithPagination;

public record GetSpeciesWithPaginationQuery(int Page, int PageSize) : IQuery;