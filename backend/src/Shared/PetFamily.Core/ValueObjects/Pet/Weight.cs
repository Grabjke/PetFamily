using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;

namespace PetFamily.Core.ValueObjects.Pet;

public record Weight
{
    //ef
    private Weight()
    {
    }
    private Weight(double value)
    {
        Value = value;
    }
    
    public double Value { get; }

    public static Result<Weight, Error> Create(double value)
    {
        if (value <= 0)
            return Errors.General.ValueIsInvalid("Weight");

        return new Weight(value);
    }
}