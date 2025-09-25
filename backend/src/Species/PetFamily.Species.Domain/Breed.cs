
using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects;

namespace PetFamily.Species.Domain;

public class Breed : SharedKernel.Entity<BreedId>
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
            return Errors.General.ValueIsInvalid("PetName");

        var breed = new Breed(breedId, name);

        return new Breed(breedId, name);
    }
}