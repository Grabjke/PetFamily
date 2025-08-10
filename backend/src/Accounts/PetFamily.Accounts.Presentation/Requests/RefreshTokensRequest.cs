using PetFamily.Accounts.Application.AccountsManagement.Commands.RefreshTokens;

namespace PetFamily.Accounts.Presentation.Requests;

public record RefreshTokensRequest(string AccessToken, Guid RefreshToken)
{
    public RefreshTokenCommand ToCommand()
        => new(AccessToken, RefreshToken);
}