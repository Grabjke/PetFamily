using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;

namespace PetFamily.Core.ValueObjects.Volunteer;

public record VolunteerDescription
{
    //ef
    private VolunteerDescription()
    {
    }
    private VolunteerDescription(string value)
    {
        Value = value;
    }
    public string Value { get; }

    public static Result<VolunteerDescription, Error> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length > Constants.MAX_HIGH_TEXT_LENGTH)
            return Errors.General.ValueIsInvalid("Description volunteer");

        return new VolunteerDescription(value);
    }
    
    
}