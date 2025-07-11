using PetFamily.Application.Abstractions;
using PetFamily.Application.Dtos;

namespace PetFamily.Application.Volunteers.Command.UpdateSocialNetworks;

public record UpdateSocialNetworksCommand(
    Guid VolunteerId,
    IEnumerable<SocialNetworkDto> SocialNetworks):ICommand;