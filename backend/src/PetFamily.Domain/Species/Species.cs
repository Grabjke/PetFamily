using PetFamily.Domain.Shared;
using PetFamily.Domain.ValueObjects.Species;

namespace PetFamily.Domain.Species;

public class PetSpecies:Entity<SpeciesId>
{
    private readonly List<Breed> _breeds = [];

    //ef core
    private PetSpecies(SpeciesId id):base(id)
    {
    }
    private PetSpecies(SpeciesId speciesId,string title):base(speciesId)
    {
        Title = title;
    }
    
    public string Title { get; private set; }
    public IReadOnlyList<Breed> Breeds => _breeds;

    

    public static Result<PetSpecies> Create(SpeciesId speciesId,string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            return Result<PetSpecies>.Failure("Title cannot be empty");
        
        var species = new PetSpecies(speciesId,title);

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