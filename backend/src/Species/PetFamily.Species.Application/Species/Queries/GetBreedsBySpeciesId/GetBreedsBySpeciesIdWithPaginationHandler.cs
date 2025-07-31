using PetFamily.Core;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Dtos.Query;
using PetFamily.Core.Extensions;
using PetFamily.Core.Models;

namespace PetFamily.Species.Application.Species.Queries.GetBreedsBySpeciesId;

public class GetBreedsBySpeciesIdWithPaginationHandler :
    IQueryHandler<PagedList<BreedDto>, GetBreedsBySpeciesIdWithPaginationQuery>
{
    private readonly ISpeciesReadDbContext _readDbContext;

    public GetBreedsBySpeciesIdWithPaginationHandler(ISpeciesReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }

    public Task<PagedList<BreedDto>> Handle(
        GetBreedsBySpeciesIdWithPaginationQuery query,
        CancellationToken cancellationToken = default)
    {
        var breeds = _readDbContext.Species
            .Where(s => s.Id == query.SpeciesId)
            .SelectMany(s => s.Breeds!); 
        
        return breeds.ToPagedList(query.Page, query.PageSize,cancellationToken);
    }
}