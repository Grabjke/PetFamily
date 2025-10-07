using System.Data;
using Microsoft.EntityFrameworkCore;
using PetFamily.Core;
using PetFamily.Discussions.Infrastructure.DbContexts;

namespace PetFamily.Discussions.Infrastructure;

public class UnitOfWork :IUnitOfWork
{
    private readonly WriteDiscussionDbContext _context;


    public UnitOfWork(WriteDiscussionDbContext context)
    {
        _context = context;
    }

    public async Task BeginTransactionAsync(
        IsolationLevel isolationLevel = IsolationLevel.ReadCommitted,
        CancellationToken cancellationToken = default)
    {
        await _context.Database.BeginTransactionAsync(isolationLevel, cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        if (_context.Database.CurrentTransaction == null)
            throw new Exception("Transaction has not been started");

        await _context.SaveChangesAsync(cancellationToken);
        await _context.Database.CommitTransactionAsync(cancellationToken);
    }

    public async Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        if (_context.Database.CurrentTransaction == null)
            throw new Exception("Transaction has not been started");

        await _context.Database.RollbackTransactionAsync(cancellationToken);
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await _context.DisposeAsync();
    }
}