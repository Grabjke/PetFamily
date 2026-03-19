using System.Data;

namespace PetFamily.Volunteers.Application;

public interface IDbConnectionFactory
{
    Task<IDbConnection> CreateConnectionAsync(CancellationToken cancellationToken = default);
}