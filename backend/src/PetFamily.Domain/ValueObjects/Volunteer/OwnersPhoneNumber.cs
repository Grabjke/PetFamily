

using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.ValueObjects.Volunteer;

public record OwnersPhoneNumber
{
    private const string PhoneNumberPattern = @"^\d{11}$"; 
    
    private OwnersPhoneNumber(string phoneNumber) 
    {
        PhoneNumber = phoneNumber;
    }
    
    public string PhoneNumber { get; }

    public static Result<OwnersPhoneNumber,Error> Create(string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
            return Errors.General.ValueIsInvalid("Phone number");
        
        if (!Regex.IsMatch(phoneNumber, PhoneNumberPattern))
            return Errors.General.ValueIsInvalid("Phone number");

        return new OwnersPhoneNumber(phoneNumber);
    }
}