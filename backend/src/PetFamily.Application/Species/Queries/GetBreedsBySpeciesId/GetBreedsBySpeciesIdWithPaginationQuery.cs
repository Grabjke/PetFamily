using PetFamily.Application.Abstractions;

namespace PetFamily.Application.Species.Queries.GetBreedsBySpeciesId;

public record GetBreedsBySpeciesIdWithPaginationQuery(
    Guid SpeciesId, 
    int Page,
    int PageSize):IQuery;