using System.Data;
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

    public async Task<IDbTransaction> BeginTransactionAsync(
        CancellationToken cancellationToken = default)
    {
        var transaction = await _volunteerDbContext.Database.BeginTransactionAsync(cancellationToken);

        return transaction.GetDbTransaction();
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _volunteerDbContext.SaveChangesAsync(cancellationToken);
    }
}