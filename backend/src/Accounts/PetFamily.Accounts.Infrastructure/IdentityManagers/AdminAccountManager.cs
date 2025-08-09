using PetFamily.Accounts.Domain;

namespace PetFamily.Accounts.Infrastructure.IdentityManagers;

public class AdminAccountManager
{
    private readonly AccountDbContext _context;

    public AdminAccountManager(AccountDbContext context)
    {
        _context = context;
    }

    public async Task CreateAdminAccount(AdminAccount adminAccount)
    {
        _context.Add(adminAccount);
        await _context.SaveChangesAsync();
    }
}