using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.ValueObjects.Pet;

public record PetColour
{
    private PetColour(string value)
    {
        Value = value;
    }
    public string Value { get; }

    public static Result<PetColour, Error> Create(string colour)
    {
        if (string.IsNullOrWhiteSpace(colour) || colour.Length > Constants.MAX_LOW_TEXT_LENGTH)
            return Errors.General.ValueIsInvalid("Colour");

        return new PetColour(colour);
    }
}