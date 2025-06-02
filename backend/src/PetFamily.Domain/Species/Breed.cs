using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.ValueObjects.Breed;

namespace PetFamily.Domain.Species;

public class Breed : Shared.Entity<BreedId>
{
    //ef core
    private Breed(BreedId id) : base(id)
    {
    }

    private Breed(BreedId breedId, string name) : base(breedId)
    {
        Name = name;
    }

    public string Name { get; private set; }

    public static Result<Breed, Error> Create(BreedId breedId, string name)
    {
        if (breedId == BreedId.Empty())
            return Errors.General.ValueIsInvalid("BreedId");

        if (string.IsNullOrWhiteSpace(name))
            return Errors.General.ValueIsInvalid("Name");

        var breed = new Breed(breedId, name);

        return new Breed(breedId, name);
    }
}