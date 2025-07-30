using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects;

namespace PetFamily.Species.Domain;

public class PetSpecies : SharedKernel.Entity<SpeciesId>
{
    private readonly List<Breed> _breeds = [];

    //ef core
    private PetSpecies(SpeciesId id) : base(id)
    {
    }

    private PetSpecies(SpeciesId speciesId, string title) : base(speciesId)
    {
        Title = title;
    }

    public string Title { get; private set; }
    public IReadOnlyList<Breed> Breeds => _breeds;

    public static Result<PetSpecies, Error> Create(SpeciesId speciesId, string title)
    {
        if (speciesId == SpeciesId.Empty())
            return Errors.General.ValueIsInvalid("SpeciesId");

        if (string.IsNullOrWhiteSpace(title))
            return Errors.General.ValueIsInvalid("Title");

        return new PetSpecies(speciesId, title);
    }

    public void DeleteBreed(Breed breed)
    {
        _breeds.Remove(breed);
    }
    public Result AddBreed(Breed breed)
    {
        if (_breeds.Contains(breed))
            return Result.Failure<PetSpecies>("Breed already exists in this species");

        _breeds.Add(breed);

        return Result.Success();
    }

    public UnitResult<Error> DeleteBreed(BreedId breedId)
    {
        var breed = _breeds.FirstOrDefault(b => b.Id.Value == breedId.Value);
        if (breed is null)
            return Errors.General.NotFound();
        
        _breeds.Remove(breed);
        
        return Result.Success<Error>();
    }
}