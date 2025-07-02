using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.ValueObjects.Breed;
using PetFamily.Domain.ValueObjects.Species;

namespace PetFamily.Application.Species;

public interface ISpeciesRepository
{
    public Task<Result<bool, Error>> VerifySpeciesAndBreedExist(
        SpeciesId speciesId,
        BreedId breedId, 
        CancellationToken cancellationToken = default);
}