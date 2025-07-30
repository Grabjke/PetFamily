

using AutoFixture;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core;
using PetFamily.Species.Infrastructure.DbContexts;

namespace PetFamily.App.IntegrationTests.Species;

public class SpeciesTestBase:IClassFixture<IntegrationTestsWebFactory>, IAsyncLifetime
{
    protected readonly IntegrationTestsWebFactory _factory;
    protected readonly Fixture _fixture;
    protected readonly IServiceScope _scope;
    protected readonly ISpeciesReadDbContext _readDbContext;
    protected readonly WriteSpeciesDbContext _writeSpeciesDbContext;
   

    protected SpeciesTestBase(IntegrationTestsWebFactory factory)
    {
        _factory = factory;
        _fixture = new Fixture();
        _scope = factory.Services.CreateScope();
        _readDbContext = _scope.ServiceProvider.GetRequiredService<ISpeciesReadDbContext>();
        _writeSpeciesDbContext = _scope.ServiceProvider.GetRequiredService<WriteSpeciesDbContext>();
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync()
    {
        _scope.Dispose();
        await _factory.ResetDatabaseAsync();
    }
}