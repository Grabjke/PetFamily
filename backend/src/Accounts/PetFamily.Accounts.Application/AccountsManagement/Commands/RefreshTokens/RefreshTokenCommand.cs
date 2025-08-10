using PetFamily.Core.Abstractions;

namespace PetFamily.Accounts.Application.AccountsManagement.Commands.RefreshTokens;

public record RefreshTokenCommand(string AccessToken, Guid RefreshToken) : ICommand;