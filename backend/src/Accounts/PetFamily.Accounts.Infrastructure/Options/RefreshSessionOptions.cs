namespace PetFamily.Accounts.Infrastructure.Options;

public class RefreshSessionOptions
{
    public const string RefreshSession = nameof(RefreshSession);
    public string ExpiredDaysTime { get; init; }
}