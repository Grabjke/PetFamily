using PetFamily.Core.ValueObjects.Volunteer;

namespace PetFamily.Accounts.Domain;

public class AdminAccount
{
    //ef
    private AdminAccount()
    {
    }
    public const string ADMIN = "Admin";
    
    public AdminAccount(FullName fullName,User user)
    {
        Id = Guid.NewGuid();
        FullName = fullName;
        User = user;
    }

    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }
    public FullName FullName { get; set; }
    
    
}