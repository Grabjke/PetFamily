using PetFamily.Accounts.Application.AccountsManagement.Commands.RefreshTokens;

namespace PetFamily.Accounts.Presentation.Requests;

public record RefreshTokensRequest(string AccessToken)
{
    public RefreshTokenCommand ToCommand(Guid refreshTokenId)
        => new(AccessToken,refreshTokenId);
}