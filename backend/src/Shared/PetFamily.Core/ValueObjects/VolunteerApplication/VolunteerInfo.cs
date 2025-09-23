using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;

namespace PetFamily.Core.ValueObjects.VolunteerApplication;

public partial class VolunteerInfo
{
    private const string PhoneNumberPattern = @"^\d{11}$";
    private const string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

    private VolunteerInfo(
        string firstName,
        string lastName,
        string? surname,
        string phoneNumber,
        string email,
        string? description)
    {
        FirstName = firstName;
        LastName = lastName;
        Surname = surname;
        PhoneNumber = phoneNumber;
        Email = email;
        Description = description;
    }
    public string FirstName { get; }
    public string LastName { get; }
    public string? Surname { get; }
    public string PhoneNumber { get; }
    public string Email { get; }
    public string? Description { get; }

    public static Result<VolunteerInfo, Error> Create(
        string firstName,
        string lastName,
        string phoneNumber,
        string email,
        string? surname = null,
        string? description = null)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            return Errors.General.ValueIsInvalid("FirstName");

        if (string.IsNullOrWhiteSpace(lastName))
            return Errors.General.ValueIsInvalid("LastName");

        if (string.IsNullOrWhiteSpace(phoneNumber))
            return Errors.General.ValueIsInvalid("Phone number");

        if (!MyRegex().IsMatch(phoneNumber))
            return Errors.General.ValueIsInvalid("Phone number");

        if (!Regex.IsMatch(email, emailPattern))
            return Errors.General.ValueIsInvalid("Email");

        return new VolunteerInfo(firstName, lastName, surname, phoneNumber, email, description);
    }

    [GeneratedRegex(PhoneNumberPattern)]
    private static partial Regex MyRegex();
}