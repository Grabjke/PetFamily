using PetFamily.Core.Abstractions;

namespace PetFamily.Accounts.Application.AccountsManagement.Commands.LoginUser;

public record LoginUserCommand(string Email, string Password) : ICommand;