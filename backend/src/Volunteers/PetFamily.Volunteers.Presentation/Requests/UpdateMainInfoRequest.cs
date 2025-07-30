using PetFamily.Core.Dtos;
using PetFamily.Volunteers.Application.Volunteers.Commands.UpdateMainInfo;

namespace PetFamily.Volunteers.Presentation.Requests;

public record UpdateMainInfoRequest(
    FullNameDto FullName,
    string Email,
    string Description,
    int Experience,
    string PhoneNumber)
{
    public UpdateMainInfoCommand ToCommand(Guid id)
        => new(id,FullName, Email, Description, Experience, PhoneNumber);
}