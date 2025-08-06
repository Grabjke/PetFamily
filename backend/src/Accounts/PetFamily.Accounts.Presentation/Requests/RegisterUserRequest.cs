using PetFamily.Accounts.Application.AccountsManagement.Commands.RegisterUser;

namespace PetFamily.Accounts.Presentation.Requests;

public record RegisterUserRequest(string UserName, string Password, string Email)
{
    public RegisterUserCommand ToCommand() => 
        new(UserName, Password, Email);
};