using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using PetFamily.Accounts.Domain;
using PetFamily.Core.Abstractions;
using PetFamily.SharedKernel;

namespace PetFamily.Accounts.Application.AccountsManagement.Commands.LoginUser;

public class LoginHandler : ICommandHandler<string, LoginUserCommand>
{
    private readonly ILogger<LoginHandler> _logger;
    private readonly UserManager<User> _userManager;
    private readonly ITokenProvider _tokenProvider;

    public LoginHandler(
        ILogger<LoginHandler> logger,
        UserManager<User> userManager,
        ITokenProvider tokenProvider)
    {
        _logger = logger;
        _userManager = userManager;
        _tokenProvider = tokenProvider;
    }

    public async Task<Result<string, ErrorList>> Handle(
        LoginUserCommand command,
        CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByEmailAsync(command.Email);
        if (user is null)
        {
            return Errors.General.NotFound().ToErrorList();
        }

        var passwordConfirmed = await _userManager.CheckPasswordAsync(user, command.Password);
        if (passwordConfirmed is false)
            return Errors.User.InvalidCredentials().ToErrorList();
        
        var token =  _tokenProvider.GenerateAccessToken(user);
        
        _logger.LogInformation("Successfully logged in");

        return token;
    }
}