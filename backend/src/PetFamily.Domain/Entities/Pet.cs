using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.ValueObjects.Pet;
using PetFamily.Domain.ValueObjects.Volunteer;

namespace PetFamily.Domain.Entities;

public class Pet : Shared.Entity<PetId>
{
    private readonly List<Requisites> _requisites = [];
    
    //ef core
    private Pet(PetId id) : base(id)
    {
    }
    
    private Pet(
        PetId id,
        string name,
        string description,
        PetSpeciesBreed speciesBreed,
        string colour,
        string healthInformation,
        Address address,
        double weight,
        double height,
        OwnersPhoneNumber phoneNumber,
        bool castration,
        DateTime birthday,
        bool isVaccinated,
        HelpStatus helpStatus
    ) : base(id)
    {
        Name = name;
        Description = description;
        PetSpeciesBreed = speciesBreed; 
        Colour = colour;
        HealthInformation = healthInformation;
        Address = address;
        Weight = weight;
        Height = height;
        OwnersPhoneNumber = phoneNumber;
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
    
    public static Result<Pet,Error> Create(
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
            return Errors.General.ValueIsInvalid("Name");
        
        if (string.IsNullOrWhiteSpace(description))
            return Errors.General.ValueIsInvalid("Description");
        
        if (string.IsNullOrWhiteSpace(colour))
            return Errors.General.ValueIsInvalid("Colour");
        
        if (string.IsNullOrWhiteSpace(healthInformation))
            return Errors.General.ValueIsInvalid("Health Information");
        
        if (weight<=0)
            return Errors.General.ValueIsInvalid("Weight");
        
        if (height<=0)
            return Errors.General.ValueIsInvalid("Height");
        
        if(birthday>DateTime.Now)
            return Errors.General.ValueIsInvalid("Birthday");

        return new Pet(petId,name, description, petSpeciesBreed, colour, healthInformation, address, weight, height,
            ownersPhoneNumber, castration, birthday, isVaccinated, helpStatus);

    }
    
    public Result AddRequisites(Requisites requisites)
    {
        if(_requisites.Contains(requisites))
            return Result.Failure<Pet>("Requisites already exists in the list");

        _requisites.Add(requisites);

        return Result.Success();
    }
}