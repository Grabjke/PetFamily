using PetFamily.Core.ValueObjects.Volunteer;

namespace PetFamily.Accounts.Domain;

public class VolunteerAccount
{
    private readonly List<Requisites> _requisites = [];
    public Guid Id { get; set; }
    public int Expirience { get; set; }
    public IReadOnlyList<Requisites> Requisites => _requisites;
    public Guid UserId { get; set; }
}