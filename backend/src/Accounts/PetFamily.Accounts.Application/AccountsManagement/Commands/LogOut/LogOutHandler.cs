using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using PetFamily.Accounts.Domain;
using PetFamily.Core.Abstractions;
using PetFamily.SharedKernel;

namespace PetFamily.Accounts.Application.AccountsManagement.Commands.LogOut;

public record LogoutCommand(Guid RefreshToken) : ICommand;

public class LogoutHandler : ICommandHandler<LogoutCommand>
{
    private const string LoginProvider = "PetFamily";
    private const string TokenName = "RefreshToken";

    private readonly UserManager<User> _userManager;
    private readonly ILogger<LogoutHandler> _logger;

    public LogoutHandler(
        UserManager<User> userManager,
        ILogger<LogoutHandler> logger)
    {
        _userManager = userManager;
        _logger = logger;
    }

    public async Task<UnitResult<ErrorList>> Handle(
        LogoutCommand command,
        CancellationToken cancellationToken = default)
    {
        // Ищем юзера по сохранённому refresh токену.
        // Если у тебя в RefreshTokensHandler юзер находится иначе — скопируй ту же логику сюда.
        var users = _userManager.Users.ToList();
        User? matchedUser = null;

        foreach (var user in users)
        {
            var stored = await _userManager.GetAuthenticationTokenAsync(
                user, LoginProvider, TokenName);

            if (stored == command.RefreshToken.ToString())
            {
                matchedUser = user;
                break;
            }
        }

        // Идемпотентность: токена уже нет — logout всё равно успешен
        if (matchedUser is null)
        {
            _logger.LogInformation(
                "Logout: refresh token {Token} not found, treating as already logged out",
                command.RefreshToken);
            return UnitResult.Success<ErrorList>();
        }

        var result = await _userManager.RemoveAuthenticationTokenAsync(
            matchedUser, LoginProvider, TokenName);

        if (!result.Succeeded)
        {
            var errors = result.Errors
                .Select(e => Error.Failure(e.Code, e.Description))
                .ToList();
            _logger.LogError("Failed to remove refresh token: {Errors}", errors);
            return new ErrorList(errors);
        }

        // Опционально — повышает безопасность, инвалидирует все остальные refresh-механизмы
        await _userManager.UpdateSecurityStampAsync(matchedUser);

        _logger.LogInformation("User {UserId} logged out", matchedUser.Id);

        return UnitResult.Success<ErrorList>();
    }
}