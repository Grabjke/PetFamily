using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Species;
using PetFamily.Domain.ValueObjects.Breed;
using PetFamily.Domain.ValueObjects.Species;

namespace PetFamily.Application.Species;

public interface ISpeciesRepository
{
    public Task<Guid> DeleteSpecies(PetSpecies species, CancellationToken cancellationToken = default);
    public Task<Result<PetSpecies,Error>> GetById(Guid speciesId,CancellationToken cancellationToken = default);
    
}