using PetFamily.Domain.Shared;

namespace PetFamily.Domain.ValueObjects.Volunteer;

public record Email
{
    

    private Email(string value)
    {
        Value = value;
    }
    public string Value { get; }

    public static Result<Email> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return "Email can not be empty";

        var email = new Email(value);
        
        return email;
    }
}