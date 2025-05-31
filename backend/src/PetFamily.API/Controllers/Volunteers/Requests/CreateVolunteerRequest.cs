using PetFamily.Application.Dtos;
using PetFamily.Application.Volunteers.CreateVolunteer;

namespace PetFamily.API.Controllers.Volunteers.Requests;

public record CreateVolunteerRequest(
    string Name,
    string Surname,
    string? Patronymic,
    string Email,
    string Description,
    int Experience,
    string PhoneNumber,
    IEnumerable<SocialNetworkDto>? SocialNetworks,
    IEnumerable<RequisitesDto>? Requisites)
{
    public CreateVolunteerCommand ToCommand() => new(
        Name,
        Surname,
        Patronymic,
        Email,
        Description,
        Experience,
        PhoneNumber,
        SocialNetworks,
        Requisites);
}
