using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.ValueObjects.Pet;

public record PetHeight
{
    //ef
    private PetHeight()
    {
    }
    private PetHeight(int number)
    {
        Height = number;
    }
    
    public int Height { get; }

    public static Result<PetHeight, Error> Create(int number)
    {
        if (number <= 0)
            return Errors.General.ValueIsInvalid("Height");

        return new PetHeight(number);
    }
}