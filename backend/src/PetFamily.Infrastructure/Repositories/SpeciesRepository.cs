using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.Application.Species;
using PetFamily.Domain.Shared;
using PetFamily.Domain.ValueObjects.Breed;
using PetFamily.Domain.ValueObjects.Species;
using PetFamily.Infrastructure.DbContexts;

namespace PetFamily.Infrastructure.Repositories;

public class SpeciesRepository : ISpeciesRepository
{
    private readonly WriteDbContext _context;

    public SpeciesRepository(WriteDbContext context)
    {
        _context = context;
    }

    public async Task<Result<bool,Error>> VerifySpeciesAndBreedExist(
        SpeciesId speciesId,
        BreedId breedId, 
        CancellationToken cancellationToken)
    {
        var result = await _context.Species
            .AnyAsync(s => s.Id == speciesId && s.Breeds
                .Any(b => b.Id == breedId), cancellationToken);

        return result;
    }
}