namespace PetFamily.Accounts.Domain;

public class ParticipantAccount
{
    //ef
    public ParticipantAccount()
    {
    }
    public const string RoleName = "Participant";
    private List<Guid> _favoritePetsIds = [];
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public IReadOnlyList<Guid> FavoritePetsIds => _favoritePetsIds;
    public void AddFavoritePet(Guid petId)
    {
        if (!_favoritePetsIds.Contains(petId))
            _favoritePetsIds.Add(petId);
    }
}