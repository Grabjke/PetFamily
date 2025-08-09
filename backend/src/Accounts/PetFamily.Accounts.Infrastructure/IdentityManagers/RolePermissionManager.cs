using Microsoft.EntityFrameworkCore;
using PetFamily.Accounts.Domain;

namespace PetFamily.Accounts.Infrastructure.IdentityManagers;

public class RolePermissionManager
{
    private readonly AccountDbContext _accountDbContext;

    public RolePermissionManager(AccountDbContext accountDbContext)
    {
        _accountDbContext = accountDbContext;
    }

    public async Task AddRangeRolePermissionIfNotExist(Guid roleId, IEnumerable<string> permissions)
    {
        foreach (var permissionCode in permissions)
        {
            var permission = await _accountDbContext.Permissions
                .FirstOrDefaultAsync(p => p.Code == permissionCode);
            if(permission is null)
                 throw new ApplicationException($"Permission with code {permissionCode} not found");

            var rolePermissionExist = await _accountDbContext.RolePermissions
                .AnyAsync(rp => rp.RoleId == roleId && rp.PermissionId == permission!.Id);

            if (rolePermissionExist)
                return;

            await _accountDbContext.RolePermissions.AddAsync(new RolePermission()
            {
                RoleId = roleId,
                PermissionId = permission!.Id
            });
        }

        await _accountDbContext.SaveChangesAsync();
    }
}