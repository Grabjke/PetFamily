using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.ValueObjects.Pet;

public record PetDescription
{
    private PetDescription(string value)
    {
        Value = value;
    }
    public string Value { get; }

    public static Result<PetDescription, Error> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length > Constants.MAX_HIGH_TEXT_LENGTH)
            return Errors.General.ValueIsInvalid("Description volunteer");

        return new PetDescription(value);
    }
}