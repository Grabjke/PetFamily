using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.Dtos.Query;
using PetFamily.Application.Extensions;
using PetFamily.Application.Models;

namespace PetFamily.Application.Species.Queries.GetBreedsBySpeciesId;

public class GetBreedsBySpeciesIdWithPaginationHandler :
    IQueryHandler<PagedList<BreedDto>, GetBreedsBySpeciesIdWithPaginationQuery>
{
    private readonly IReadDbContext _readDbContext;

    public GetBreedsBySpeciesIdWithPaginationHandler(IReadDbContext readDbContext)
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