using PetFamily.Application.Dtos;
using PetFamily.Application.Volunteers.Command.UpdateSocialNetworks;

namespace PetFamily.API.Controllers.Volunteers.Requests;

public record UpdateSocialNetworksRequest(
    IEnumerable<SocialNetworkDto> SocialNetworks)
{
    public UpdateSocialNetworksCommand ToCommand(Guid id)
        => new(id, SocialNetworks);
}