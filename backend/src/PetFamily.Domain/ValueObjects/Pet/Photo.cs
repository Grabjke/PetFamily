namespace PetFamily.Domain.ValueObjects.Pet;

public record Photo
{
    //ef
    private Photo() { }
    
    public Photo(FilePath pathToStorage)
    {
        PathToStorage = pathToStorage;
    }

    public FilePath PathToStorage { get; }
}