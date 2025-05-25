using PetFamily.Domain.Shared;
using PetFamily.Domain.ValueObjects.Breed;
using PetFamily.Domain.ValueObjects.Species;

namespace PetFamily.Domain.ValueObjects.Pet;

public record PetSpeciesBreed
{
    private PetSpeciesBreed(SpeciesId speciesId,BreedId breedId)
    {
        SpeciesId = speciesId;
        BreedId = breedId;
    }
    public SpeciesId SpeciesId { get; }
    public BreedId BreedId { get; }

    public static Result<PetSpeciesBreed> Create(SpeciesId speciesId,BreedId breedId)
    {
        if (speciesId == SpeciesId.Empty())
            return "SpeciesId cannot be empty";
        
        if (breedId == BreedId.Empty())
            return "BreedId cannot be empty";

        var petSpeciesBreed = new PetSpeciesBreed(speciesId, breedId);

        return petSpeciesBreed;
    }
}