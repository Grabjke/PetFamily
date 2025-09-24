using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PetFamily.Accounts.Application.AccountsManagement.Commands.LoginUser;
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
        [FromServices] RegisterUserHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request.ToCommand(), cancellationToken);

        return result.IsFailure
            ? result.Error.ToResponse()
            : Ok();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(
        [FromBody] LoginUserRequest request,
        [FromServices] LoginHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request.ToCommand(), cancellationToken);

        SetRefreshTokenCookie(result.Value.RefreshToken.ToString());

        return result.IsFailure
            ? result.Error.ToResponse()
            : Ok(new
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

        SetRefreshTokenCookie(result.Value.RefreshToken.ToString());

        return result.IsFailure
            ? result.Error.ToResponse()
            : Ok(new
            {
                accessToken = result.Value.AccessToken,
                refreshToken = result.Value.RefreshToken,
                roles = result.Value.Roles,
                email = result.Value.Email,
                userId = result.Value.UserId,
            });
    }

    private void SetRefreshTokenCookie(string refreshToken)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            IsEssential = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddDays(int.Parse(options.Value.ExpiredDaysTime)),
        };

        Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
    }
}