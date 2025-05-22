using PetFamily.Domain.Shared;
using PetFamily.Domain.ValueObjects.Pet;
using PetFamily.Domain.ValueObjects.Volunteer;

namespace PetFamily.Domain.Entities;

public class Pet : Entity<PetId>
{
    private readonly List<Requisites> _requisites = [];
    private Pet(PetId id):base(id)
    {
    }
    
    private Pet(
        PetId petId,
        string name,
        string description,
        PetSpeciesBreed petSpeciesBreed,
        string colour,
        string healthInformation,
        Address address,
        double weight,
        double height,
        OwnersPhoneNumber ownersPhoneNumber,
        bool castration,
        DateTime birthday,
        bool isVaccinated,
        HelpStatus helpStatus
    ) : base(petId)
    {
        Name = name;
        Description = description;
        PetSpeciesBreed = petSpeciesBreed;
        Colour = colour;
        HealthInformation = healthInformation;
        Address = address;
        Weight = weight;
        Height = height;
        OwnersPhoneNumber = ownersPhoneNumber;
        Castration = castration;
        Birthday = birthday;
        IsVaccinated = isVaccinated;
        HelpStatus = helpStatus;
        DateOfCreation = DateTime.UtcNow;
    }
    
    
    public string Name { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public PetSpeciesBreed PetSpeciesBreed { get; private set; } 
    public string Colour { get; private set; } = null!;
    public string HealthInformation { get; private set; } = null!;
    public Address Address { get; private set; } = null!;
    public double Weight { get; private set; }
    public double Height { get; private set; }
    public OwnersPhoneNumber OwnersPhoneNumber { get; private set; }
    public bool Castration { get; private set; }
    public DateTime Birthday { get; private set; }
    public bool IsVaccinated { get; private set; }
    public HelpStatus HelpStatus { get; private set; }
    public IReadOnlyList<Requisites> Requisites  => _requisites;
    public DateTime DateOfCreation { get; private set; }
    
    //ef core
   

    public static Result<Pet> Create(
        PetId petId,
        string name,
        string description,
        PetSpeciesBreed petSpeciesBreed,
        string colour,
        string healthInformation,
        Address address,
        double weight,
        double height,
        OwnersPhoneNumber ownersPhoneNumber,
        bool castration,
        DateTime birthday,
        bool isVaccinated,
        HelpStatus helpStatus)
    {
        if (string.IsNullOrWhiteSpace(name))
            return "Name cannot be empty";
        
        if (string.IsNullOrWhiteSpace(description))
            return "Description cannot be empty";
        
        if (string.IsNullOrWhiteSpace(colour))
            return "Colour cannot be empty";
        
        if (string.IsNullOrWhiteSpace(healthInformation))
            return "HealthInformation cannot be empty";
        
        if (weight<=0)
            return "Weight is invalid";
        
        if (height<=0)
            return "Height is invalid";
        
        if(birthday>DateTime.Now)
            return "Birthday is invalid";

        var pet = new Pet(petId,name, description, petSpeciesBreed, colour, healthInformation, address, weight, height,
            ownersPhoneNumber, castration, birthday, isVaccinated, helpStatus);

        return pet;

    }
    
    public Result AddRequisites(Requisites requisites)
    {
        if(_requisites.Contains(requisites))
            return "Requisites already exists in the list";

        _requisites.Add(requisites);

        return Result.Success();
    }
}