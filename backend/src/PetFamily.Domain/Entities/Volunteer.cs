using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.ValueObjects.Pet;
using PetFamily.Domain.ValueObjects.Volunteer;

namespace PetFamily.Domain.Entities;

public class Volunteer: Shared.Entity<VolunteerId>
{
    private readonly List<Pet> _pets = [];
    private readonly List<SocialNetwork> _socialNetworks = [];
    private readonly List<Requisites> _requisites = [];
    
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
        OwnersPhoneNumber phoneNumber
        ):base(volunteerId)
    {
        FullName = fullName;
        Email = email;
        Description = description;
        Experience = experience;
        PhoneNumber = phoneNumber;

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
    public IReadOnlyList<Requisites> Requisites => _requisites;
    public IReadOnlyList<Pet> Pets => _pets;


    public static Result<Volunteer,Error> Create( 
        VolunteerId volunteerId,
        FullName fullName,
        Email email,
        string description,
        int experience,
        OwnersPhoneNumber phoneNumber
        )
    {
        
        if (string.IsNullOrWhiteSpace(description))
            return Errors.General.ValueIsInvalid("Description");
        
        if (experience<0)
            return Errors.General.ValueIsInvalid("Experience");

        return new Volunteer(volunteerId,fullName, email, description, experience, phoneNumber);

    }
    
    public Result AddPet(Pet pet)
    {
        if(_pets.Contains(pet))
            return Result.Failure<Volunteer>("Pet already exists in the list");

        _pets.Add(pet);

        return Result.Success();
    }
    
    public UnitResult<Error> AddRequisites(Requisites requisites)
    {
       if(_requisites.Contains(requisites))
            return Errors.General.ValueIsInvalid("Requisites");

        _requisites.Add(requisites);

        return Result.Success<Error>();
    }
    public UnitResult<Error> AddSocialNetwork(SocialNetwork socialNetwork)
    {
        if((_socialNetworks).Contains(socialNetwork))
            return Errors.General.ValueIsInvalid("Social network");

        _socialNetworks.Add(socialNetwork);
        return Result.Success<Error>();
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