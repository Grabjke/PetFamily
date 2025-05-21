
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Species;

public class Breed
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }

   
    private Breed(string name)
    {
        Id=Guid.NewGuid();
        Name = name;
    }

    public static Result<Breed> Create(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return "Name cannot be empty";
        
        var breed = new Breed(name);
        
        return breed;
    }
}