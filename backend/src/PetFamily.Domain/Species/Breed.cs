
using PetFamily.Domain.Shared;
using PetFamily.Domain.ValueObjects.Breed;

namespace PetFamily.Domain.Species;

public class Breed:Entity<BreedId>
{
    //ef core
    private Breed(BreedId id):base(id)
    {
    }
    private Breed(BreedId breedId,string name):base(breedId)
    {
        Name = name;
    }
    public string Name { get; private set; }

    public static Result<Breed> Create(BreedId breedId,string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return "Name cannot be empty";
        
        var breed = new Breed(breedId,name);
        
        return breed;
    }
}