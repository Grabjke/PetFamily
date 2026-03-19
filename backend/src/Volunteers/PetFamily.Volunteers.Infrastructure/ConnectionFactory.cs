using System.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Npgsql;
using PetFamily.Volunteers.Application;

namespace PetFamily.Volunteers.Infrastructure;

public class ConnectionFactory : IDisposable, IAsyncDisposable, IDbConnectionFactory
{
    private readonly NpgsqlDataSource _dataSource;

    public ConnectionFactory(IConfiguration configuration)
    {
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(configuration.GetConnectionString("Database"));

        dataSourceBuilder.UseLoggerFactory(CreateLoggerFactory());

        _dataSource = dataSourceBuilder.Build();
    }

    public async Task<IDbConnection> CreateConnectionAsync(CancellationToken cancellationToken = default)
    {
        return await _dataSource.OpenConnectionAsync(cancellationToken);
    }

    private ILoggerFactory CreateLoggerFactory() =>
        LoggerFactory.Create(builder => { builder.AddConsole(); });

    public void Dispose()
    {
        _dataSource.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await _dataSource.DisposeAsync();
    }
}