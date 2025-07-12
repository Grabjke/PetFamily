using PetFamily.Application.Species.Queries.GetBreedsBySpeciesId;

namespace PetFamily.API.Controllers.Species.Requests;

public record GetBreedsBySpeciesIdWithPaginationRequest(int Page,int PageSize)
{
    public GetBreedsBySpeciesIdWithPaginationQuery ToQuery(Guid speciesId)
        =>new(speciesId, Page, PageSize);
}