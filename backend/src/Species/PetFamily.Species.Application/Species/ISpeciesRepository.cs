using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;
using PetFamily.Species.Domain;

namespace PetFamily.Species.Application.Species;

public interface ISpeciesRepository
{
    public Task<Guid> DeleteSpecies(
        PetSpecies species, 
        CancellationToken cancellationToken = default);
    public Task<Result<PetSpecies,Error>> GetById(
        Guid speciesId,
        CancellationToken cancellationToken = default);
    
}