using CSharpFunctionalExtensions;
using PetFamily.Accounts.Domain;
using PetFamily.SharedKernel;

namespace PetFamily.Accounts.Application;

public interface IRefreshSessionManager
{
    Task<Result<RefreshSession, Error>> GetByRefreshToken(
        Guid refreshTokenId,
        CancellationToken cancellationToken);

    void Delete(RefreshSession refreshSession);
}