using PetFamily.Accounts.Application.AccountsManagement.Commands.LoginUser;

namespace PetFamily.Accounts.Presentation.Requests;

public record LoginUserRequest(string Email, string Password)
{
    public LoginUserCommand ToCommand() =>
        new(Email, Password);
};