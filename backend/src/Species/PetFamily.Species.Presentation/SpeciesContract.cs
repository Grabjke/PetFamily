using Microsoft.EntityFrameworkCore;
using PetFamily.Core;
using PetFamily.Species.Contracts;
using PetFamily.Species.Contracts.Requests;

namespace PetFamily.Species.Presentation;

public class SpeciesContract : ISpeciesContract
{
    private readonly ISpeciesReadDbContext _readDbContext;

    public SpeciesContract(ISpeciesReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }
    public async Task<bool> IsSpeciesAndBreedExist(
        IsSpeciesAndBreedExistRequest request,
        CancellationToken cancellationToken)
    {
        return await _readDbContext.Species
            .Where(s => s.Id == request.SpeciesId)
            .SelectMany(s => s.Breeds!)
            .AnyAsync(b => b.Id == request.BreedId, cancellationToken);
    }
}