using PetFamily.Core.Abstractions;

namespace PetFamily.Accounts.Application.AccountsManagement.Commands.RefreshTokens;

public record RefreshTokenCommand(Guid RefreshToken) : ICommand;