namespace PetFamily.Core.Dtos;

public record VolunteerInfoDto(
    string FirstName,
    string LastName,
    string? Surname,
    string PhoneNumber,
    string Email,
    string? Description);
