using AutoFixture;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Database;
using PetFamily.Infrastructure.DbContexts;

namespace PetFamily.App.IntegrationTests.Species;

public class SpeciesTestBase:IClassFixture<IntegrationTestsWebFactory>, IAsyncLifetime
{
    protected readonly IntegrationTestsWebFactory _factory;
    protected readonly Fixture _fixture;
    protected readonly IServiceScope _scope;
    protected readonly IReadDbContext _readDbContext;
    protected readonly WriteDbContext _writeDbContext;
   

    protected SpeciesTestBase(IntegrationTestsWebFactory factory)
    {
        _factory = factory;
        _fixture = new Fixture();
        _scope = factory.Services.CreateScope();
        _readDbContext = _scope.ServiceProvider.GetRequiredService<IReadDbContext>();
        _writeDbContext = _scope.ServiceProvider.GetRequiredService<WriteDbContext>();
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync()
    {
        _scope.Dispose();
        await _factory.ResetDatabaseAsync();
    }
}