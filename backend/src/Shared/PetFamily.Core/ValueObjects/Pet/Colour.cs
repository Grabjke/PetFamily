using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;

namespace PetFamily.Core.ValueObjects.Pet;

public record Colour
{
    private Colour(string value)
    {
        Value = value;
    }
    public string Value { get; }

    public static Result<Colour, Error> Create(string colour)
    {
        if (string.IsNullOrWhiteSpace(colour) || colour.Length > Constants.MAX_LOW_TEXT_LENGTH)
            return Errors.General.ValueIsInvalid("Colour");

        return new Colour(colour);
    }
}