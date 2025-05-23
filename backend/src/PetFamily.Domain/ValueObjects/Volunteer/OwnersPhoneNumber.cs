﻿

using PetFamily.Domain.Shared;

namespace PetFamily.Domain.ValueObjects.Volunteer;

public record OwnersPhoneNumber
{
    private const int PHONE_LENGTH = 11;
    
    

    private OwnersPhoneNumber(string phoneNumber) 
    {
        PhoneNumber = phoneNumber;
    }
    public string PhoneNumber { get; }

    public static Result<OwnersPhoneNumber> Create(string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber) || phoneNumber.Length != PHONE_LENGTH)
            return "Phone number invalid";

        var ownersPhoneNumber = new OwnersPhoneNumber(phoneNumber);

        return ownersPhoneNumber;
    }
}