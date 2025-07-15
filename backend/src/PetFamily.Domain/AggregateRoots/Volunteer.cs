using CSharpFunctionalExtensions;
using PetFamily.Domain.Entities;
using PetFamily.Domain.Shared;
using PetFamily.Domain.ValueObjects.Pet;
using PetFamily.Domain.ValueObjects.Volunteer;

namespace PetFamily.Domain.AggregateRoots;

public class Volunteer : SoftDeletableEntity<VolunteerId>
{
    private readonly List<Pet> _pets = [];
    private readonly List<SocialNetwork> _socialNetworks = [];
    private readonly List<Requisites> _requisites = [];

    //ef core
    private Volunteer(VolunteerId id) : base(id)
    {
    }

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

    public override void Delete()
    {
        base.Delete();
        foreach (var pet in _pets)
        {
            pet.Delete();
        }
    }

    public override void Restore()
    {
        base.Restore();
        foreach (var pet in _pets)
        {
            pet.Restore();
        }
    }

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

    public void HardDeletePet(Pet pet)
    {
        _pets.Remove(pet);
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

    public Result<Pet, Error> GetPetById(PetId petId)
    {
        var pet = _pets.FirstOrDefault(x => x.Id == petId);
        if(pet is null)
            return Errors.General.NotFound();
        
        return pet;
    }

    public UnitResult<Error> AddPet(Pet pet)
    {
        if (_pets.Contains(pet))
            return Errors.General.AllReadyExist();

        var serialNumberResult = Position.Create(_pets.Count + 1);
        if (serialNumberResult.IsFailure)
            return Errors.General.AllReadyExist();

        pet.SetSerialNumber(serialNumberResult.Value);

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

    public UnitResult<Error> MovePet(Pet pet, Position newPosition)
    {
        var currentPosition = pet.Position;

        if (currentPosition == newPosition || _pets.Count == 1)
            return Result.Success<Error>();

        var adjustPosition = AdjustNewPositionIfOutOfRange(newPosition);
        if (adjustPosition.IsFailure)
            return adjustPosition.Error;

        newPosition = adjustPosition.Value;

        var moveResult = MovePetBetweenPositions(newPosition, currentPosition);
        if (moveResult.IsFailure)
            return moveResult.Error;

        pet.Move(newPosition);

        return Result.Success<Error>();
    }

    private UnitResult<Error> MovePetBetweenPositions(Position newPosition, Position currentPosition)
    {
        if (newPosition < currentPosition)
        {
            var petsToMove = _pets.Where(p => p.Position >= newPosition
                                              && p.Position <= currentPosition);

            foreach (var petToMove in petsToMove)
            {
                var result = petToMove.MoveForward();
                if (result.IsFailure)
                {
                    return result.Error;
                }
            }
        }
        else if (newPosition > currentPosition)
        {
            var petsToMove = _pets.Where(p => p.Position > currentPosition
                                              && p.Position <= newPosition);

            foreach (var petToMove in petsToMove)
            {
                var result = petToMove.MoveBack();
                if (result.IsFailure)
                {
                    return result.Error;
                }
            }
        }

        return Result.Success<Error>();
    }

    private Result<Position, Error> AdjustNewPositionIfOutOfRange(Position newPosition)
    {
        if (newPosition.Value <= _pets.Count)
            return newPosition;

        var lastPosition = Position.Create(_pets.Count);
        if (lastPosition.IsFailure)
            return lastPosition.Error;

        return lastPosition.Value;
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