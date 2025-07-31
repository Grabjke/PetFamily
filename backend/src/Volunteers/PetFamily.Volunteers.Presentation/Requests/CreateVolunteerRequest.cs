using PetFamily.Core.Dtos;
using PetFamily.Volunteers.Application.Volunteers.Commands.Create;

namespace PetFamily.Volunteers.Presentation.Requests;

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
