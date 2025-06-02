using PetFamily.Application.Dtos;

namespace PetFamily.Application.Volunteers.CreateVolunteer;

public record CreateVolunteerCommand(
    FullNameDto FullName,
    string Email,
    string Description,
    int Experience,
    string PhoneNumber,
    IEnumerable<SocialNetworkDto>? SocialNetworks,
    IEnumerable<RequisitesDto>? Requisites);
    

