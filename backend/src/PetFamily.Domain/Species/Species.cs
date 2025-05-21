using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Species;

public class PetSpecies
{
    private readonly List<Breed> _breeds = [];
    
    public Guid Id { get; private set; }
    public string Title { get; private set; }
    public IReadOnlyList<Breed> Breeds => _breeds;

    private PetSpecies(string title)
    {
        Id = Guid.NewGuid();
        Title = title;
    }

    public static Result<PetSpecies> Create(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            return Result<PetSpecies>.Failure("Title cannot be empty");
        
        var species = new PetSpecies(title);

        return Result<PetSpecies>.Success(species);
    }
    
    public Result AddBreed(Breed breed)
    {
        if(_breeds.Contains(breed))
            return "Breed already exists in this species";

        _breeds.Add(breed);

        return Result.Success();
    }
    

}