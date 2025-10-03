using PetFamily.Core.Dtos;
using PetFamily.VolunteersApplications.Application.VolunteersApplications.Commands.Create;

namespace PetFamily.VolunteersApplications.Presentation.Requests;

public record CreateApplicationRequest(
    Guid UserId,
    string FirstName,
    string LastName,
    string? Surname,
    string PhoneNumber,
    string Email,
    string? Description)
{
    public CreateApplicationCommand ToCommand() =>
        new(UserId, new VolunteerInfoDto(FirstName, LastName, Surname, PhoneNumber, Email, Description));
}