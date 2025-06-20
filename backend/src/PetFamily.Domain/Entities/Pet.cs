﻿using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.ValueObjects.Pet;
using PetFamily.Domain.ValueObjects.Volunteer;

namespace PetFamily.Domain.Entities;

public class Pet : SoftDeletableEntity<PetId>
{
    private readonly List<Requisites> _requisites = [];
    
    //ef core
    private Pet(PetId id) : base(id) { }
    
    public Pet(
        PetId id,
        PetName name,
        PetDescription description,
        PetSpeciesBreed speciesBreed,
        PetColour colour,
        PetHealthInformation healthInformation,
        Address address,
        PetWeight weight,
        PetHeight height,
        OwnersPhoneNumber phoneNumber,
        bool castration,
        Birthday birthday,
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
    
    public PetName Name { get; private set; } = null!;
    public PetDescription Description { get; private set; } = null!;
    public PetSpeciesBreed PetSpeciesBreed { get; private set; } 
    public PetColour Colour { get; private set; } = null!;
    public PetHealthInformation HealthInformation { get; private set; } = null!;
    public Address Address { get; private set; } = null!;
    public PetWeight Weight { get; private set; }
    public PetHeight Height { get; private set; }
    public OwnersPhoneNumber OwnersPhoneNumber { get; private set; }
    public bool Castration { get; private set; }
    public Birthday Birthday { get; private set; }
    public bool IsVaccinated { get; private set; }
    public HelpStatus HelpStatus { get; private set; }
    public IReadOnlyList<Requisites> Requisites  => _requisites;
    public DateTime DateOfCreation { get; private set; }
    
    public UnitResult<Error> AddRequisites(Requisites requisites)
    {
        if(_requisites.Contains(requisites))
            return Errors.General.AllReadyExist();

        _requisites.Add(requisites);

        return Result.Success<Error>();
    }
}