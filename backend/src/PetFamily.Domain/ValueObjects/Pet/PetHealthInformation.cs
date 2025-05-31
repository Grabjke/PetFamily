using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.ValueObjects.Pet;

public record PetHealthInformation
{
    private PetHealthInformation(string value)
    {
        Value = value;
    }
    public string Value { get; }

    public static Result<PetHealthInformation, Error> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length > Constants.MAX_HIGH_TEXT_LENGTH)
            return Errors.General.ValueIsInvalid("Health Information");

        return new PetHealthInformation(value);
    }
}