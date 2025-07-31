using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects;

namespace PetFamily.Volunteers.Domain.PetManagement.ValueObjects.Pet;

public record PetSpeciesBreed
{
    private PetSpeciesBreed(SpeciesId speciesId,BreedId breedId)
    {
        SpeciesId = speciesId;
        BreedId = breedId;
    }
    public SpeciesId SpeciesId { get; }
    public BreedId BreedId { get; }

    public static Result<PetSpeciesBreed,Error> Create(SpeciesId speciesId,BreedId breedId)
    {
        if (speciesId == SpeciesId.Empty())
            return Errors.General.ValueIsInvalid("SpeciesId");
        
        if (breedId == BreedId.Empty())
            return Errors.General.ValueIsInvalid("BreedId");

        return new PetSpeciesBreed(speciesId, breedId);
    }
}