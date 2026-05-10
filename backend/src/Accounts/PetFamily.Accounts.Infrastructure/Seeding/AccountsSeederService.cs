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
    private const string SeedPassword = ":&1vwiWj@ksYRgPJe/";
    private const string SeedAdminEmail = "admin@gmail.com";
    private const string SeedAdminUserName = "admin";
    private const string SeedVolunteerEmail = "volunteer@gmail.com";
    private const string SeedVolunteerUserName = "volunteer";
    private const string VolunteerRoleName = "Volunteer";

    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly AdminAccountManager _adminAccountManager;
    private readonly PermissionManager _permissionManager;
    private readonly RolePermissionManager _rolePermissionManager;
    private readonly AdminOptions _adminOptions;
    private readonly ILogger<AccountsSeederService> _logger;

    public AccountsSeederService(
        UserManager<User> userManager,
        RoleManager<Role> roleManager,
        AdminAccountManager adminAccountManager,
        PermissionManager permissionManager,
        RolePermissionManager rolePermissionManager,
        IOptions<AdminOptions> adminOptions,
        ILogger<AccountsSeederService> logger)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _adminAccountManager = adminAccountManager;
        _permissionManager = permissionManager;
        _rolePermissionManager = rolePermissionManager;
        _adminOptions = adminOptions.Value;
        _logger = logger;
    }

    public async Task SeedAsync()
    {
        var json = await File.ReadAllTextAsync(FilePaths.Accounts);

        var seedData = JsonSerializer.Deserialize<RolePermissionOptions>(json)
                       ?? throw new ApplicationException("Json doesn't contain role permission config");

        await SeedPermissions(seedData);
        await SeedRoles(seedData);
        await SeedRolePermissions(seedData);

        var adminRole = await _roleManager.FindByNameAsync(AdminAccount.ADMIN)
                        ?? throw new ApplicationException("Admin role doesn't exist");

        var volunteerRole = await _roleManager.FindByNameAsync(VolunteerRoleName)
                            ?? throw new ApplicationException("Volunteer role doesn't exist");

        // Админ из AdminOptions (как было)
        await SeedAdminFromOptions(adminRole);

        // Хардкод admin@gmail.com
        await SeedAdminIfNotExists(
            email: SeedAdminEmail,
            userName: SeedAdminUserName,
            password: SeedPassword,
            adminRole: adminRole);

        // Хардкод volunteer@gmail.com
        await SeedVolunteerIfNotExists(
            email: SeedVolunteerEmail,
            userName: SeedVolunteerUserName,
            password: SeedPassword,
            volunteerRole: volunteerRole);
    }

    private async Task SeedAdminFromOptions(Role adminRole)
    {
        var existing = await _userManager.FindByEmailAsync(_adminOptions.Email);
        if (existing is not null)
        {
            _logger.LogInformation("Admin from options already exists: {Email}", _adminOptions.Email);
            return;
        }

        var adminUser = User.CreateAdmin(_adminOptions.UserName, _adminOptions.Email, adminRole);
        var createResult = await _userManager.CreateAsync(adminUser, _adminOptions.Password);
        if (!createResult.Succeeded)
        {
            _logger.LogError("Failed to create admin {Email}: {Errors}",
                _adminOptions.Email,
                string.Join("; ", createResult.Errors.Select(e => e.Description)));
            return;
        }

        var fullname = FullName.Create(_adminOptions.UserName, _adminOptions.UserName).Value;
        var adminAccount = new AdminAccount(fullname, adminUser);
        await _adminAccountManager.CreateAdminAccount(adminAccount);

        _logger.LogInformation("Seeded admin from options: {Email}", _adminOptions.Email);
    }

    private async Task SeedAdminIfNotExists(string email, string userName, string password, Role adminRole)
    {
        var existing = await _userManager.FindByEmailAsync(email);
        if (existing is not null)
        {
            _logger.LogInformation("Admin already exists: {Email}", email);
            return;
        }

        var adminUser = User.CreateAdmin(userName, email, adminRole);
        var createResult = await _userManager.CreateAsync(adminUser, password);
        if (!createResult.Succeeded)
        {
            _logger.LogError("Failed to create admin {Email}: {Errors}",
                email,
                string.Join("; ", createResult.Errors.Select(e => e.Description)));
            return;
        }

        var fullname = FullName.Create(userName, userName).Value;
        var adminAccount = new AdminAccount(fullname, adminUser);
        await _adminAccountManager.CreateAdminAccount(adminAccount);

        _logger.LogInformation("Seeded admin: {Email}", email);
    }

    private async Task SeedVolunteerIfNotExists(string email, string userName, string password, Role volunteerRole)
    {
        var existing = await _userManager.FindByEmailAsync(email);
        if (existing is not null)
        {
            _logger.LogInformation("Volunteer already exists: {Email}", email);
            return;
        }

        var user = User.CreateAdmin(userName, email, volunteerRole);

        var createResult = await _userManager.CreateAsync(user, password);
        if (!createResult.Succeeded)
        {
            _logger.LogError("Failed to create volunteer {Email}: {Errors}",
                email,
                string.Join("; ", createResult.Errors.Select(e => e.Description)));
            return;
        }

        _logger.LogInformation("Seeded volunteer: {Email}", email);
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