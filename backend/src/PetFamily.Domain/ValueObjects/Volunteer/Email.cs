using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.ValueObjects.Volunteer;

public record Email
{
    
    private Email(string value)
    {
        Value = value;
    }
    public string Value { get; }

    public static Result<Email,Error> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Errors.General.ValueIsInvalid("Email");
        
        return new Email(value);
    }
}