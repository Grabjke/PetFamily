using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.ValueObjects.Pet;
using PetFamily.Domain.ValueObjects.Volunteer;

namespace PetFamily.Domain.Entities;

public class Volunteer : Shared.Entity<VolunteerId>
{
    private readonly List<Pet> _pets = [];
    private readonly List<SocialNetwork> _socialNetworks = [];
    private readonly List<Requisites> _requisites = [];

    //ef core
    private Volunteer(VolunteerId id) : base(id) { }

    public Volunteer(
        VolunteerId volunteerId,
        FullName fullName,
        Email email,
        VolunteerDescription description,
        VolunteerExperience experience,
        OwnersPhoneNumber phoneNumber
    ) : base(volunteerId)
    {
        FullName = fullName;
        Email = email;
        Description = description;
        Experience = experience;
        PhoneNumber = phoneNumber;
    }

    public FullName FullName { get; private set; }
    public Email Email { get; private set; }
    public VolunteerDescription Description { get; private set; }
    public VolunteerExperience Experience { get; private set; }
    public int PetsFoundHome => GetCountPetsFoundHome();
    public int PetsLookingForHome => GetCountPetsLookingForHome();
    public int PetsNeedsHelp => GetCountPetsNeedsHelp();
    public OwnersPhoneNumber PhoneNumber { get; private set; }
    public IReadOnlyList<SocialNetwork> SocialNetworks => _socialNetworks;
    public IReadOnlyList<Requisites> Requisites => _requisites;
    public IReadOnlyList<Pet> Pets => _pets;

    public void UpdateMainInfo(
        FullName fullName,
        Email email,
        VolunteerDescription description,
        VolunteerExperience experience,
        OwnersPhoneNumber phoneNumber)
    {
        FullName = fullName;
        Email = email;
        Description = description;
        Experience = experience;
        PhoneNumber = phoneNumber;
    }

    public UnitResult<Error> UpdateRequisites(List<Requisites> requisites)
    {
        var newRequisites = new List<Requisites>();

        foreach (var requisite in requisites)
        {
            if (newRequisites.Contains(requisite))
                return Errors.General.AllReadyExist();
            
            newRequisites.Add(requisite);
        }
        
        _requisites.Clear();
        _requisites.AddRange(newRequisites);
        
        return Result.Success<Error>();
    }
    public UnitResult<Error> UpdateSocialNetworks(List<SocialNetwork> socialNetworks)
    {
        var newSocialNetworks = new List<SocialNetwork>();

        foreach (var socialNetwork in socialNetworks)
        {
            if (newSocialNetworks.Contains(socialNetwork))
                return Errors.General.AllReadyExist();
            
            newSocialNetworks.Add(socialNetwork);
        }
        
        _socialNetworks.Clear();
        _socialNetworks.AddRange(newSocialNetworks);
        
        return Result.Success<Error>();
    }

    public UnitResult<Error> AddPet(Pet pet)
    {
        if (_pets.Contains(pet))
            return Errors.General.AllReadyExist();

        _pets.Add(pet);

        return Result.Success<Error>();
    }

    public UnitResult<Error> AddRequisites(Requisites requisites)
    {
        if (_requisites.Contains(requisites))
            return Errors.General.AllReadyExist();

        _requisites.Add(requisites);

        return Result.Success<Error>();
    }

    public UnitResult<Error> AddSocialNetwork(SocialNetwork socialNetwork)
    {
        if (_socialNetworks.Contains(socialNetwork))
            return Errors.General.AllReadyExist();

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