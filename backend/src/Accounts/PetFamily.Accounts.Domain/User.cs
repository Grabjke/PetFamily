using Microsoft.AspNetCore.Identity;
using PetFamily.Core.ValueObjects.Volunteer;

namespace PetFamily.Accounts.Domain;

public class User : IdentityUser<Guid>
{
    private User()
    {
    }

    private List<Role> _roles = [];
    public IReadOnlyList<Role> Roles => _roles;
    public IReadOnlyList<SocialNetwork> SocialNetworks { get; set; } = [];
    public ParticipantAccount? ParticipantAccount { get; set; }

    public static User CreateAdmin(string username, string email, Role role)
    {
        return new User()
        {
            UserName = username,
            Email = email,
            _roles = [role]
        };
    }

    public static User CreateParticipantAccount(
        string userName,
        string email,
        Role role)
    {
        var user = new User()
        {
            Email = email,
            UserName = userName,
            _roles = [role]
        };

        var participant = new ParticipantAccount();

        user.ParticipantAccount = participant;

        return user;
    }

    public void AddRole(List<Role> roles)
    {
        _roles.AddRange(roles);
    }
}