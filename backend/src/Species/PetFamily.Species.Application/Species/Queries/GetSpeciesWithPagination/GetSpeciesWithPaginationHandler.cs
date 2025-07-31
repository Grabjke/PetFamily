using PetFamily.Core;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Dtos.Query;
using PetFamily.Core.Extensions;
using PetFamily.Core.Models;

namespace PetFamily.Species.Application.Species.Queries.GetSpeciesWithPagination;

public class GetSpeciesWithPaginationHandler : 
    IQueryHandler<PagedList<SpeciesDto>, GetSpeciesWithPaginationQuery>
{
    private readonly ISpeciesReadDbContext _readDbContext;

    public GetSpeciesWithPaginationHandler(ISpeciesReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }

    public Task<PagedList<SpeciesDto>> Handle(
        GetSpeciesWithPaginationQuery query,
        CancellationToken cancellationToken = default)
    {
        var species = _readDbContext.Species;

        return species.ToPagedList(query.Page, query.PageSize, cancellationToken: cancellationToken);
    }
}