using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Accounts.Contracts;
using PetFamily.Core;
using PetFamily.Core.Models;

namespace PetFamily.Framework.Authorization;

public class PermissionRequirementHandler : AuthorizationHandler<PermissionAttribute>
{
    private readonly IServiceScopeFactory _scopeFactory;

    public PermissionRequirementHandler(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionAttribute permission)
    {
        using var scope = _scopeFactory.CreateScope();

        var accountContract = scope.ServiceProvider.GetRequiredService<IAccountContract>();

        var userIdString = context.User.Claims
            .FirstOrDefault(c => c.Type == CustomClaims.Id)?.Value;

        if (!Guid.TryParse(userIdString, out var userId))
        {
            return;
        }

        var permissions = await accountContract.GetUserPermissionsCode(userId);

        if (permissions.Contains(permission.Code))
        {
            context.Succeed(permission);
        }
    }
}