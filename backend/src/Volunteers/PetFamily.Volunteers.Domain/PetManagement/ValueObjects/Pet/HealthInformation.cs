using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;

namespace PetFamily.Volunteers.Domain.PetManagement.ValueObjects.Pet;

public record HealthInformation
{
    private HealthInformation(string value)
    {
        Value = value;
    }
    public string Value { get; }

    public static Result<HealthInformation, Error> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length > Constants.MAX_HIGH_TEXT_LENGTH)
            return Errors.General.ValueIsInvalid("Health Information");

        return new HealthInformation(value);
    }
}