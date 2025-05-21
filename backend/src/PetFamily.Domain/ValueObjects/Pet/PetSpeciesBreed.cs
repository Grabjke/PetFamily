using PetFamily.Domain.Shared;

namespace PetFamily.Domain.ValueObjects.Pet;

public record PetSpeciesBreed
{
    public Guid SpeciesId { get; }
    public Guid BreedId { get; }

    private PetSpeciesBreed(Guid speciesId,Guid breedId)
    {
        SpeciesId = speciesId;
        BreedId = breedId;
    }

    public static Result<PetSpeciesBreed> Create(Guid speciesId, Guid breedId)
    {
        if (speciesId == Guid.Empty)
            return "SpeciesId cannot be empty";
        
        if (breedId == Guid.Empty)
            return "BreedId cannot be empty";

        var petSpeciesBreed = new PetSpeciesBreed(speciesId, breedId);

        return petSpeciesBreed;
    }
}