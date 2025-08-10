using Microsoft.EntityFrameworkCore;
using PetFamily.Accounts.Domain;

namespace PetFamily.Accounts.Infrastructure.IdentityManagers;

public class PermissionManager
{
    private readonly AccountDbContext _accountDbContext;

    public PermissionManager(AccountDbContext accountDbContext)
    {
        _accountDbContext = accountDbContext;
    }

    public async Task AddRangeIfNotExist(IEnumerable<string> permissions)
    {
        foreach (var permissionCode in permissions)
        {
            var isPermissionExist = await _accountDbContext.Permissions
                .AnyAsync(p => p.Code == permissionCode);
            if (isPermissionExist)
            {
                return;
            }

            await _accountDbContext.AddAsync(new Permission() { Code = permissionCode });
        }

        await _accountDbContext.SaveChangesAsync();
    }

    public async Task<Permission?> FindByCode(string code)
    {
        return await _accountDbContext.Permissions
            .FirstOrDefaultAsync(p => p.Code == code);
    }

    public async Task<HashSet<string>> GetUserPermissionsCode(Guid userId)
    {
        var permissions = await _accountDbContext.Users
            .Include(u => u.Roles)
            .Where(u => u.Id == userId)
            .SelectMany(u => u.Roles)
            .SelectMany(r=>r.RolePermissions)
            .Select(rp=>rp.Permission.Code)
            .ToListAsync();
        
        return permissions.ToHashSet();
    }
}