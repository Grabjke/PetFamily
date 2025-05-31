using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.ValueObjects.Pet;

public record PetWeight
{
    //ef
    private PetWeight()
    {
    }
    private PetWeight(double number)
    {
        Weight = number;
    }
    
    public double Weight { get; }

    public static Result<PetWeight, Error> Create(double number)
    {
        if (number <= 0)
            return Errors.General.ValueIsInvalid("Weight");

        return new PetWeight(number);
    }
}