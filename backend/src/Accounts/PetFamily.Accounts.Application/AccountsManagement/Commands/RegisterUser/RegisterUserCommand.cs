using PetFamily.Core.Abstractions;

namespace PetFamily.Accounts.Application.AccountsManagement.Commands.RegisterUser;

public record RegisterUserCommand(string UserName, string Password, string Email) : ICommand;