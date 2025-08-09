using Microsoft.Extensions.DependencyInjection;

namespace PetFamily.Accounts.Infrastructure.Seeding;

public class AccountsSeeder
{
    private readonly IServiceScopeFactory _scopeFactory;

    public AccountsSeeder(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public async Task SeedAsync()
    {
        using var scope = _scopeFactory.CreateScope();
        
        var services = scope.ServiceProvider.GetRequiredService<AccountsSeederService>();
        
        await services.SeedAsync();
    }
    
}