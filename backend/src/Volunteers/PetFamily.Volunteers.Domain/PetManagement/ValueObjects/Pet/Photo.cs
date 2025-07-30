
using PetFamily.Core;

namespace PetFamily.Volunteers.Domain.PetManagement.ValueObjects.Pet;

public record Photo
{
    //ef
    private Photo()
    {
    }

    public Photo(FilePath pathToStorage, bool isMain = false)
    {
        PathToStorage = pathToStorage;
        IsMain = isMain;
    }
    
    public FilePath PathToStorage { get; }

    public bool IsMain { get; set; }

    public void SetAsMain() => IsMain = true;
    public void UnsetAsMain() => IsMain = false;
}