using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PetFamily.Accounts.Application.AccountsManagement.Commands.LoginUser;
using PetFamily.Accounts.Application.AccountsManagement.Commands.LogOut;
using PetFamily.Accounts.Application.AccountsManagement.Commands.RefreshTokens;
using PetFamily.Accounts.Application.AccountsManagement.Commands.RegisterUser;
using PetFamily.Accounts.Infrastructure.Options;
using PetFamily.Accounts.Presentation.Requests;
using PetFamily.Framework;
using PetFamily.SharedKernel;

namespace PetFamily.Accounts.Presentation;

public class AccountController(IOptions<RefreshSessionOptions> options) : ApplicationController
{
    [HttpPost("registration")]
    public async Task<IActionResult> Register(
        [FromBody] RegisterUserRequest request,
        [FromServices] RegisterUserHandler registerHandler,
        [FromServices] LoginHandler loginHandler,
        CancellationToken cancellationToken)
    {
        var registerResult = await registerHandler.Handle(request.ToCommand(), cancellationToken);
        if (registerResult.IsFailure)
            return registerResult.Error.ToResponse();

        // Сразу логиним пользователя, чтобы вернуть токены и поставить cookie
        var loginResult = await loginHandler.Handle(
            new LoginUserCommand(request.Email, request.Password),
            cancellationToken);

        if (loginResult.IsFailure)
            return loginResult.Error.ToResponse();

        SetRefreshTokenCookie(loginResult.Value.RefreshToken.ToString());

        return Ok(new
        {
            accessToken = loginResult.Value.AccessToken,
            refreshToken = loginResult.Value.RefreshToken,
            roles = loginResult.Value.Roles,
            email = loginResult.Value.Email,
            userId = loginResult.Value.UserId,
        });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(
        [FromBody] LoginUserRequest request,
        [FromServices] LoginHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request.ToCommand(), cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        SetRefreshTokenCookie(result.Value.RefreshToken.ToString());

        return Ok(new
        {
            accessToken = result.Value.AccessToken,
            refreshToken = result.Value.RefreshToken,
            roles = result.Value.Roles,
            email = result.Value.Email,
            userId = result.Value.UserId,
        });
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshTokens(
        [FromServices] RefreshTokensHandler handler,
        CancellationToken cancellationToken)
    {
        var refreshToken = Request.Cookies["refreshToken"];
        if (string.IsNullOrEmpty(refreshToken))
            return Unauthorized(Errors.Tokens.InvalidToken().ToResponse());

        if (!Guid.TryParse(refreshToken, out var refreshTokenGuid))
            return Unauthorized(Errors.Tokens.InvalidToken().ToResponse());

        var result = await handler.Handle(new RefreshTokenCommand(refreshTokenGuid), cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        SetRefreshTokenCookie(result.Value.RefreshToken.ToString());

        return Ok(new
        {
            accessToken = result.Value.AccessToken,
            refreshToken = result.Value.RefreshToken,
            roles = result.Value.Roles,
            email = result.Value.Email,
            userId = result.Value.UserId,
        });
    }
    
    [HttpPost("logout")]
    public async Task<IActionResult> Logout(
        [FromServices] LogoutHandler handler,
        CancellationToken cancellationToken)
    {
        var refreshToken = Request.Cookies["refreshToken"];

        if (!string.IsNullOrEmpty(refreshToken)
            && Guid.TryParse(refreshToken, out var refreshTokenGuid))
        {
            var result = await handler.Handle(
                new LogoutCommand(refreshTokenGuid),
                cancellationToken);

            if (result.IsFailure)
                return result.Error.ToResponse();
        }

        DeleteRefreshTokenCookie();
        return NoContent();
    }

    private void DeleteRefreshTokenCookie()
    {
        // Опции должны совпадать с Set, иначе браузер не удалит cookie
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            IsEssential = true,
            SameSite = SameSiteMode.None,
        };

        Response.Cookies.Delete("refreshToken", cookieOptions);
    }

    private void SetRefreshTokenCookie(string refreshToken)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,                       // обязательно при SameSite=None
            IsEssential = true,
            SameSite = SameSiteMode.None,        // было Strict — cookie не ходил cross-origin
            Expires = DateTime.UtcNow.AddDays(int.Parse(options.Value.ExpiredDaysTime)),
        };

        Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
    }
}