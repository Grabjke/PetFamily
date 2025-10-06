using PetFamily.Core.ValueObjects.Volunteer;

namespace PetFamily.Accounts.Domain;

public class VolunteerAccount
{
    public const string RoleName = "Volunteer";
    public VolunteerAccount()
    {
    }

    private readonly List<Requisites> _requisites = [];
    public Guid Id { get; set; }
    public int Experience { get; set; }
    public IReadOnlyList<Requisites> Requisites => _requisites;
    public Guid UserId { get; set; }
}