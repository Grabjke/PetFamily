using PetFamily.Application.Abstractions;
using PetFamily.Application.Dtos;

namespace PetFamily.Application.Volunteers.Command.Create;

public record CreateVolunteerCommand(
    FullNameDto FullName,
    string Email,
    string Description,
    int Experience,
    string PhoneNumber,
    IEnumerable<SocialNetworkDto>? SocialNetworks,
    IEnumerable<RequisitesDto>? Requisites) : ICommand;