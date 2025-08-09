using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using PetFamily.Core;
using PetFamily.Species.Infrastructure.DbContexts;

namespace PetFamily.Species.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private readonly WriteSpeciesDbContext _speciesDbContext;

    public UnitOfWork(WriteSpeciesDbContext speciesDbContext)
    {
        _speciesDbContext = speciesDbContext;
    }

    public async Task BeginTransactionAsync(
        IsolationLevel isolationLevel = IsolationLevel.ReadCommitted,
        CancellationToken cancellationToken = default)
    {
        await _speciesDbContext.Database.BeginTransactionAsync(isolationLevel, cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _speciesDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        if (_speciesDbContext.Database.CurrentTransaction == null)
            throw new Exception("Transaction has not been started");

        await _speciesDbContext.SaveChangesAsync(cancellationToken);
        await _speciesDbContext.Database.CommitTransactionAsync(cancellationToken);
    }

    public async Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        if (_speciesDbContext.Database.CurrentTransaction == null)
            throw new Exception("Transaction has not been started");

        await _speciesDbContext.Database.RollbackTransactionAsync(cancellationToken);
    }

    public void Dispose()
    {
        _speciesDbContext.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await _speciesDbContext.DisposeAsync();
    }
}