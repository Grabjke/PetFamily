namespace PetFamily.Accounts.Contracts;

public interface IAccountContract
{
    Task<HashSet<string>> GetUserPermissionsCode(Guid userId);
}