using PetFamily.Accounts.Domain;

namespace PetFamily.Accounts.Infrastructure.IdentityManagers;

public class AccountsManager
{
    private readonly AccountDbContext _context;

    public AccountsManager(AccountDbContext context)
    {
        _context = context;
    }

    public async Task CreateAdminAccount(AdminAccount adminAccount)
    {
        _context.Add(adminAccount);
        await _context.SaveChangesAsync();
    }
}