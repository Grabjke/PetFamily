using PetFamily.Application.Dtos;
using PetFamily.Application.Volunteers.Command.Create;

namespace PetFamily.API.Controllers.Volunteers.Requests;

public record CreateVolunteerRequest(
    FullNameDto FullName,
    string Email,
    string Description,
    int Experience,
    string PhoneNumber,
    IEnumerable<SocialNetworkDto>? SocialNetworks,
    IEnumerable<RequisitesDto>? Requisites)
{
    public CreateVolunteerCommand ToCommand() => new(
        FullName,
        Email,
        Description,
        Experience,
        PhoneNumber,
        SocialNetworks,
        Requisites);
}
