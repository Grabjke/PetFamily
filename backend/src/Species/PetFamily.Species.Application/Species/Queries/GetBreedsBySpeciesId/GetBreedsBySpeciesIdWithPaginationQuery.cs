using PetFamily.Core.Abstractions;

namespace PetFamily.Species.Application.Species.Queries.GetBreedsBySpeciesId;

public record GetBreedsBySpeciesIdWithPaginationQuery(
    Guid SpeciesId, 
    int Page,
    int PageSize):IQuery;