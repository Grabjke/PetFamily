using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.Application.Species;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Species;
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
    

    public async Task<Guid> DeleteSpecies(PetSpecies species, CancellationToken cancellationToken)
    {
        _context.Species.Remove(species);

        await _context.SaveChangesAsync(cancellationToken);

        return species.Id.Value;
    }

    public async Task<Guid> DeleteBreed(Breed breed, CancellationToken cancellationToken = default)
    {
        _context.Entry(breed).State = EntityState.Deleted;
        
        await _context.SaveChangesAsync(cancellationToken);
        
        return breed.Id.Value;
    }

    public async Task<Result<PetSpecies, Error>> GetById(
        Guid speciesId,
        CancellationToken cancellationToken = default)
    {
        var searchId = SpeciesId.Create(speciesId);

        var record = await _context.Species
            .Include(s => s.Breeds)
            .FirstOrDefaultAsync(s => s.Id == searchId, cancellationToken);

        if (record is null)
            return Errors.General.NotFound();

        return record;
    }
}