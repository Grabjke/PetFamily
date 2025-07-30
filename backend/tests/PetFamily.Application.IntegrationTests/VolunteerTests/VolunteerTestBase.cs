

using AutoFixture;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core;
using PetFamily.Species.Infrastructure.DbContexts;
using PetFamily.Volunteers.Infrastructure.DbContexts;

namespace PetFamily.App.IntegrationTests.VolunteerTests;


public class VolunteerTestBase : IClassFixture<IntegrationTestsWebFactory>, IAsyncLifetime
{
    protected readonly IntegrationTestsWebFactory _factory;
    protected readonly Fixture _fixture;
    protected readonly IServiceScope _scope;
    protected readonly IVolunteersReadDbContext _readVolunteerDbContext;
    
    protected readonly WriteVolunteerDbContext WriteVolunteerDbContext;
    protected readonly WriteSpeciesDbContext _writeSpeciesDbContext;
   

    protected VolunteerTestBase(IntegrationTestsWebFactory factory)
    {
        _factory = factory;
        _fixture = new Fixture();
        _scope = factory.Services.CreateScope();
        _readVolunteerDbContext = _scope.ServiceProvider.GetRequiredService<IVolunteersReadDbContext>();
        WriteVolunteerDbContext = _scope.ServiceProvider.GetRequiredService<WriteVolunteerDbContext>();
        _writeSpeciesDbContext= _scope.ServiceProvider.GetRequiredService<WriteSpeciesDbContext>();
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync()
    {
        _scope.Dispose();
        await _factory.ResetDatabaseAsync();
    }
}