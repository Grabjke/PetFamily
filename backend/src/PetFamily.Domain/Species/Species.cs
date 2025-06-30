using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.ValueObjects.Species;

namespace PetFamily.Domain.Species;

public class PetSpecies: Shared.Entity<SpeciesId>
{
    private readonly List<Breed> _breeds = [];

    //ef core
    private PetSpecies(SpeciesId id):base(id){ }
    private PetSpecies(SpeciesId speciesId,string title):base(speciesId)
    {
        Title = title;
    }
    
    public string Title { get; private set; }
    public IReadOnlyList<Breed> Breeds => _breeds;
    
    public static Result<PetSpecies,Error> Create(SpeciesId speciesId,string title)
    {
        if (speciesId == SpeciesId.Empty())
            return Errors.General.ValueIsInvalid("SpeciesId");
        
        if (string.IsNullOrWhiteSpace(title))
            return Errors.General.ValueIsInvalid("Title");

        return new PetSpecies(speciesId,title);
    }
    
    public Result AddBreed(Breed breed)
    {
        if(_breeds.Contains(breed))
            return Result.Failure<PetSpecies>("Breed already exists in this species");

        _breeds.Add(breed);

        return Result.Success();
    }
    

}