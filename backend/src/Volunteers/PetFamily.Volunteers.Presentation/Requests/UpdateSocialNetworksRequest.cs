using PetFamily.Core.Dtos;
using PetFamily.Volunteers.Application.Volunteers.Commands.UpdateSocialNetworks;

namespace PetFamily.Volunteers.Presentation.Requests;

public record UpdateSocialNetworksRequest(
    IEnumerable<SocialNetworkDto> SocialNetworks)
{
    public UpdateSocialNetworksCommand ToCommand(Guid id)
        => new(id, SocialNetworks);
}