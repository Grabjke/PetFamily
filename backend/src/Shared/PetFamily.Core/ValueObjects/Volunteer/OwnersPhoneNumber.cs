using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;

namespace PetFamily.Core.ValueObjects.Volunteer;

public partial record OwnersPhoneNumber
{
    private const string PhoneNumberPattern = @"^\d{11}$"; 
    
    private OwnersPhoneNumber(string value) 
    {
        Value = value;
    }
    
    public string Value { get; }

    public static Result<OwnersPhoneNumber,Error> Create(string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
            return Errors.General.ValueIsInvalid("Phone number");
        
        if (!MyRegex().IsMatch(phoneNumber))
            return Errors.General.ValueIsInvalid("Phone number");

        return new OwnersPhoneNumber(phoneNumber);
    }

    [GeneratedRegex(PhoneNumberPattern)]
    private static partial Regex MyRegex();
}