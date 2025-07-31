using System.Data.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Npgsql;
using PetFamily.Core;
using PetFamily.Species.Infrastructure.DbContexts;
using PetFamily.Volunteers.Infrastructure.DbContexts;
using PetFamily.Web;
using Respawn;
using Testcontainers.PostgreSql;

namespace PetFamily.App.IntegrationTests;

public class IntegrationTestsWebFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithImage("postgres")
        .WithDatabase("pet_family_tests")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .Build();

    private Respawner _respawner = default!;
    private DbConnection _dbConnection = default!;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(ConfigureDefaultServices);
    }

    protected virtual void ConfigureDefaultServices(IServiceCollection services)
    {
        services.RemoveAll<IVolunteersReadDbContext>();
        services.RemoveAll<ISpeciesReadDbContext>();
        services.RemoveAll<WriteVolunteerDbContext>();
        services.RemoveAll<WriteSpeciesDbContext>();

        var connectionString = _dbContainer.GetConnectionString();

        services.AddScoped<WriteVolunteerDbContext>(_ => new WriteVolunteerDbContext(connectionString));
        services.AddScoped<WriteSpeciesDbContext>(_ => new WriteSpeciesDbContext(connectionString));
        services.AddScoped<IVolunteersReadDbContext>(_ =>
            new VolunteerReadDbContext(connectionString));
        services.AddScoped<ISpeciesReadDbContext>(_ =>
            new SpeciesReadDbContext(connectionString));
    }

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();

        using var scope = Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<WriteVolunteerDbContext>();
        var dbContextSpecies = scope.ServiceProvider.GetRequiredService<WriteSpeciesDbContext>();
        await dbContext.Database.MigrateAsync();
        await dbContextSpecies.Database.MigrateAsync();

        _dbConnection = new NpgsqlConnection(_dbContainer.GetConnectionString());

        await InitializeRespawner();
    }

    private async Task InitializeRespawner()
    {
        await _dbConnection.OpenAsync();

        _respawner = await Respawner.CreateAsync(_dbConnection, new RespawnerOptions
            {
                DbAdapter = DbAdapter.Postgres,
                SchemasToInclude = ["public"]
            }
        );
    }

    public async Task ResetDatabaseAsync()
    {
        await _respawner.ResetAsync(_dbConnection);
    }

    public new async Task DisposeAsync()
    {
        await _dbContainer.StopAsync();
        await _dbContainer.DisposeAsync();
    }
}