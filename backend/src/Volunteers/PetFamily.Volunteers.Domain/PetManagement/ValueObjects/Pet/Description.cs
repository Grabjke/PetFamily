using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;

namespace PetFamily.Volunteers.Domain.PetManagement.ValueObjects.Pet;

public record Description
{
    private Description(string value)
    {
        Value = value;
    }
    public string Value { get; }

    public static Result<Description, Error> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length > Constants.MAX_HIGH_TEXT_LENGTH)
            return Errors.General.ValueIsInvalid("Description volunteer");

        return new Description(value);
    }
}