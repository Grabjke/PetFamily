using PetFamily.Accounts.Domain;

namespace PetFamily.Accounts.Application.Authorization;

public interface ITokenProvider
{
    string GenerateAccessToken(User user);
}