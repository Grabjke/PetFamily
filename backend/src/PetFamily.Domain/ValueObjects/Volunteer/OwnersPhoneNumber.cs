

using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.ValueObjects.Volunteer;

public record OwnersPhoneNumber
{
    private const int PHONE_LENGTH = 10;
    
    private OwnersPhoneNumber(string phoneNumber) 
    {
        PhoneNumber = phoneNumber;
    }
    public string PhoneNumber { get; }

    public static Result<OwnersPhoneNumber,Error> Create(string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber) || phoneNumber.Length != PHONE_LENGTH)
            return Errors.General.ValueIsInvalid("Phone number");
        

        return new OwnersPhoneNumber(phoneNumber);
    }
}