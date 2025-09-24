using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using PetFamily.Accounts.Contracts.Responses;
using PetFamily.Accounts.Domain;
using PetFamily.Core.Abstractions;
using PetFamily.SharedKernel;

namespace PetFamily.Accounts.Application.AccountsManagement.Commands.LoginUser;

public class LoginHandler : ICommandHandler<LoginResponse, LoginUserCommand>
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

    public async Task<Result<LoginResponse, ErrorList>> Handle(
        LoginUserCommand command,
        CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByEmailAsync(command.Email);
        if (user is null)
        {
            return Errors.General.NotFound().ToErrorList();
        }

        var roles = await _userManager.GetRolesAsync(user);

        var passwordConfirmed = await _userManager.CheckPasswordAsync(user, command.Password);
        if (passwordConfirmed is false)
            return Errors.User.InvalidCredentials().ToErrorList();

        var accessToken = _tokenProvider.GenerateAccessToken(user);
        var refreshToken = await _tokenProvider.GenerateRefreshToken(user, accessToken.Jti, cancellationToken);

        _logger.LogInformation("Successfully logged in");

        return new LoginResponse(
            accessToken.AccessToken,
            refreshToken,
            user.Id, 
            user.Email!,
            roles.Select(r=>r.ToLower()));
    }
}