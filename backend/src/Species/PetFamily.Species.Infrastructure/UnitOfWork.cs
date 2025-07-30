using System.Data;
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

    public async Task<IDbTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        var transaction = await _speciesDbContext.Database.BeginTransactionAsync(cancellationToken);

        return transaction.GetDbTransaction();
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _speciesDbContext.SaveChangesAsync(cancellationToken);
    }
}