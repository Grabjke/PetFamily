using PetFamily.Domain.Shared;
using PetFamily.Domain.ValueObjects.Pet;
using PetFamily.Domain.ValueObjects.Volunteer;

namespace PetFamily.Domain.Entities;

public class Volunteer: Entity<VolunteerId>
{
    private readonly List<Pet> _pets = [];
    private readonly List<SocialNetwork> _socialNetworks = [];
    
    //ef core
    private Volunteer(VolunteerId id):base(id)
    {
    }

    private Volunteer(
        VolunteerId volunteerId,
        FullName fullName,
        Email email,
        string description,
        int experience,
        OwnersPhoneNumber phoneNumber,
        Requisites requisites
        ):base(volunteerId)
    {
        FullName = fullName;
        Email = email;
        Description = description;
        Experience = experience;
        PhoneNumber = phoneNumber;
        Requisites = requisites;

    }
    public FullName FullName { get; private set; }
    public Email Email { get; private set; }
    public string Description { get; private set; }
    public int Experience { get; private set; }
    public int PetsFoundHome => GetCountPetsFoundHome();
    public int PetsLookingForHome => GetCountPetsLookingForHome();
    public int PetsNeedsHelp => GetCountPetsNeedsHelp();
    public OwnersPhoneNumber PhoneNumber { get; private set; }
    public IReadOnlyList<SocialNetwork> SocialNetworks => _socialNetworks;
    public Requisites Requisites { get; private set; }
    public IReadOnlyList<Pet> Pets => _pets;


    public static Result<Volunteer> Create( 
        VolunteerId volunteerId,
        FullName fullName,
        Email email,
        string description,
        int experience,
        OwnersPhoneNumber phoneNumber,
        Requisites requisites
        )
    {
        
        if (string.IsNullOrWhiteSpace(description))
            return "Description cannot be empty";
        
        if (experience<0)
            return "Experience is invalid";

        var volunteer = new Volunteer(volunteerId,fullName, email, description, experience, phoneNumber, requisites);

        return volunteer;

    }
    
    public Result AddPet(Pet pet)
    {
        if(_pets.Contains(pet))
            return "Pet already exists in the list";

        _pets.Add(pet);

        return Result.Success();
    }
    
    public Result AddSocialNetwork(SocialNetwork socialNetwork)
    {
        if(_socialNetworks.Contains(socialNetwork))
            return "Social network already exists in the list";

        _socialNetworks.Add(socialNetwork);

        return Result.Success();
    }
    
    private int GetCountPetsFoundHome()
    {
        return _pets.Count(x => x.HelpStatus == HelpStatus.FoundHome);
    }
    
    private int GetCountPetsLookingForHome()
    {
        return _pets.Count(x => x.HelpStatus == HelpStatus.LookingForHome);
    }
    
    private int GetCountPetsNeedsHelp()
    {
        return _pets.Count(x => x.HelpStatus == HelpStatus.NeedsHelp);
    }
    
}