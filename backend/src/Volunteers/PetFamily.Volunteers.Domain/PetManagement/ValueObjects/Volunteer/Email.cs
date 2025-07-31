using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using PetFamily.Core;
using PetFamily.SharedKernel;

namespace PetFamily.Volunteers.Domain.PetManagement.ValueObjects.Volunteer;

public record Email
{
    private Email(string value)
    {
        Value = value;
    }

    public string Value { get; }


    public static Result<Email, Error> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Errors.General.ValueIsRequired("Email");


        value = value.Trim();


        if (value.Length > 254)
            return Errors.General.ValueIsInvalid("Email");


        string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        if (!Regex.IsMatch(value, emailPattern))
            return Errors.General.ValueIsInvalid("Email");


        return new Email(value);
    }
}