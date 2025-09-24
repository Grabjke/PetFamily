using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.Accounts.Application;
using PetFamily.Accounts.Domain;
using PetFamily.SharedKernel;

namespace PetFamily.Accounts.Infrastructure.IdentityManagers;

public class RefreshSessionManager : IRefreshSessionManager
{
    private readonly AccountDbContext _context;

    public RefreshSessionManager(AccountDbContext context)
    {
        _context = context;
    }

    public async Task<Result<RefreshSession, Error>> GetByRefreshToken(
        Guid refreshTokenId,
        CancellationToken cancellationToken)
    {
        var refreshSession = await _context.RefreshSessions
            .Include(r => r.User)
            .ThenInclude(u=>u.Roles)
            .FirstOrDefaultAsync(rs => rs.RefreshToken == refreshTokenId, cancellationToken);

        if (refreshSession is null)
            return Errors.General.NotFound();

        return refreshSession;
    }

    public void Delete(RefreshSession refreshSession)
    {
        _context.RefreshSessions.Remove(refreshSession);
    }
}