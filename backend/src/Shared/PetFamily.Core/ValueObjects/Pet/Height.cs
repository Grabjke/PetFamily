using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;

namespace PetFamily.Core.ValueObjects.Pet;

public record Height
{
    //ef
    private Height()
    {
    }
    private Height(int value)
    {
        Value = value;
    }
    
    public int Value { get; }

    public static Result<Height, Error> Create(int value)
    {
        if (value <= 0)
            return Errors.General.ValueIsInvalid("Height");

        return new Height(value);
    }
}