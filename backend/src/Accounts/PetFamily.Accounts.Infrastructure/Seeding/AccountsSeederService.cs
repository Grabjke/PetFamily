using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PetFamily.Accounts.Domain;
using PetFamily.Accounts.Infrastructure.IdentityManagers;
using PetFamily.Accounts.Infrastructure.Options;
using PetFamily.Core.ValueObjects.Volunteer;
using PetFamily.Framework;

namespace PetFamily.Accounts.Infrastructure.Seeding;

public class AccountsSeederService
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly AccountsManager _accountsManager;
    private readonly PermissionManager _permissionManager;
    private readonly RolePermissionManager _rolePermissionManager;
    private readonly AdminOptions _adminOptions;
    private readonly ILogger<AccountsSeederService> _logger;

    public AccountsSeederService(
        UserManager<User> userManager,
        RoleManager<Role> roleManager,
        AccountsManager accountsManager,
        PermissionManager permissionManager,
        RolePermissionManager rolePermissionManager,
        IOptions<AdminOptions> adminOptions,
        ILogger<AccountsSeederService> logger)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _accountsManager = accountsManager;
        _permissionManager = permissionManager;
        _rolePermissionManager = rolePermissionManager;
        _adminOptions = adminOptions.Value;
        _logger = logger;
    }

    public async Task SeedAsync()
    {
        if (await AnyAdminExistsAsync())
        {
            return; 
        }
    }
    
    private async Task<bool> AnyAdminExistsAsync()
    {
        var adminRole = await _roleManager.FindByNameAsync(AdminAccount.ADMIN);
        if (adminRole == null) return false;
    
        var admins = await _userManager.GetUsersInRoleAsync(adminRole.Name);
        
        return admins.Any();
    }

    private async Task SeedRolePermissions(RolePermissionOptions seedData)
    {
        foreach (var roleName in seedData.Roles.Keys)
        {
            var role = await _roleManager.FindByNameAsync(roleName);

            var rolePermissions = seedData.Roles[roleName];

            await _rolePermissionManager.AddRangeRolePermissionIfNotExist(role!.Id, rolePermissions);
        }

        _logger.LogInformation("Seeding role permissions");
    }

    private async Task SeedRoles(RolePermissionOptions seedData)
    {
        foreach (var roleName in seedData.Roles.Keys)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role is null)
            {
                await _roleManager.CreateAsync(new Role() { Name = roleName });
            }
        }

        _logger.LogInformation("Seeding roles");
    }

    private async Task SeedPermissions(RolePermissionOptions seedData)
    {
        var permissionsToAdd = seedData.Permissions
            .SelectMany(permissionGroup => permissionGroup.Value);

        await _permissionManager.AddRangeIfNotExist(permissionsToAdd);

        _logger.LogInformation("Seeding permissions");
    }
}