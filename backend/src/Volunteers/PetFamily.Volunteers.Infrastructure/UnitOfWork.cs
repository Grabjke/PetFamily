using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using PetFamily.Core;
using PetFamily.Volunteers.Infrastructure.DbContexts;

namespace PetFamily.Volunteers.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private readonly WriteVolunteerDbContext _volunteerDbContext;

    public UnitOfWork(WriteVolunteerDbContext volunteerDbContext)
    {
        _volunteerDbContext = volunteerDbContext;
    }

    public async Task BeginTransactionAsync(
        IsolationLevel isolationLevel = IsolationLevel.ReadCommitted,
        CancellationToken cancellationToken = default)
    {
        await _volunteerDbContext.Database.BeginTransactionAsync(isolationLevel, cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _volunteerDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        if (_volunteerDbContext.Database.CurrentTransaction == null)
            throw new Exception("Transaction has not been started");

        await _volunteerDbContext.SaveChangesAsync(cancellationToken);
        await _volunteerDbContext.Database.CommitTransactionAsync(cancellationToken);
    }

    public async Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        if (_volunteerDbContext.Database.CurrentTransaction == null)
            throw new Exception("Transaction has not been started");

        await _volunteerDbContext.Database.RollbackTransactionAsync(cancellationToken);
    }

    public void Dispose()
    {
        _volunteerDbContext.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await _volunteerDbContext.DisposeAsync();
    }
}