using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects;
using PetFamily.Species.Application.Species;
using PetFamily.Species.Domain;
using PetFamily.Species.Infrastructure.DbContexts;

namespace PetFamily.Species.Infrastructure;

public class SpeciesRepository : ISpeciesRepository
{
    private readonly WriteSpeciesDbContext _context;
    public SpeciesRepository(WriteSpeciesDbContext context)
    {
        _context = context;
    }
    
    public async Task<Guid> DeleteSpecies(PetSpecies species, CancellationToken cancellationToken)
    {
        _context.Species.Remove(species);

        await _context.SaveChangesAsync(cancellationToken);

        return species.Id.Value;
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