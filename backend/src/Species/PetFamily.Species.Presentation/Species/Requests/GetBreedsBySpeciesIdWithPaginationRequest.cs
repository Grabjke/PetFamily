using PetFamily.Species.Application.Species.Queries.GetBreedsBySpeciesId;

namespace PetFamily.Species.Presentation.Species.Requests;

public record GetBreedsBySpeciesIdWithPaginationRequest(int Page,int PageSize)
{
    public GetBreedsBySpeciesIdWithPaginationQuery ToQuery(Guid speciesId)
        =>new(speciesId, Page, PageSize);
}