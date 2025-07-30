using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects;
using PetFamily.Volunteers.Domain.PetManagement.ValueObjects.Pet;
using PetFamily.Volunteers.Domain.PetManagement.ValueObjects.Volunteer;

namespace PetFamily.Volunteers.Domain.PetManagement;

public class Pet : SoftDeletableEntity<PetId>
{
    private readonly List<Requisites> _requisites = [];
    private readonly List<Photo> _photos = [];

    //ef core
    private Pet(PetId id) : base(id)
    {
    }

    public Pet(
        PetId id,
        PetName name,
        Description description,
        PetSpeciesBreed speciesBreed,
        Colour colour,
        HealthInformation healthInformation,
        Address address,
        Weight weight,
        Height height,
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
    public Description Description { get; private set; } = null!;
    public PetSpeciesBreed PetSpeciesBreed { get; private set; }
    public Colour Colour { get; private set; } = null!;
    public HealthInformation HealthInformation { get; private set; } = null!;
    public Address Address { get; private set; } = null!;
    public Weight Weight { get; private set; }
    public Height Height { get; private set; }
    public OwnersPhoneNumber OwnersPhoneNumber { get; private set; }
    public bool Castration { get; private set; }
    public Birthday Birthday { get; private set; }
    public bool IsVaccinated { get; private set; }
    public HelpStatus HelpStatus { get; private set; }
    public IReadOnlyList<Requisites> Requisites => _requisites;
    public IReadOnlyList<Photo> Photos => _photos;
    public DateTime DateOfCreation { get; private set; }
    public Position Position { get; private set; }

    public override void Delete()
    {
        if (IsDeleted) return;

        base.Delete();
    }

    public override void Restore()
    {
        if (!IsDeleted) return;

        base.Restore();
    }

    internal UnitResult<Error> SetMainPhoto(string path)
    {
        var photoExists = _photos.Any(p => p.PathToStorage.Path == path);
        if (!photoExists)
            return Errors.General.NotFound();
        
        var updatedPhotos = new List<Photo>();
    
        foreach (var photo in _photos)
        {
            var isMain = photo.PathToStorage.Path == path;
            var newPhoto = new Photo(photo.PathToStorage, isMain);
            updatedPhotos.Add(newPhoto);
        }
        
        _photos.Clear();
        _photos.AddRange(updatedPhotos);

        return UnitResult.Success<Error>();
    }

    internal UnitResult<Error> AddRequisites(Requisites requisites)
    {
        if (_requisites.Contains(requisites))
            return Errors.General.AllReadyExist();

        _requisites.Add(requisites);

        return Result.Success<Error>();
    }

    internal void UpdateMainInfo(
        PetName name,
        Description description,
        PetSpeciesBreed speciesBreed,
        Colour colour,
        HealthInformation healthInformation,
        Address address,
        Weight weight,
        Height height,
        OwnersPhoneNumber phoneNumber,
        bool castration,
        Birthday birthday,
        bool isVaccinated,
        HelpStatus helpStatus)
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
    }

    internal UnitResult<Error> ChangeStatus(int status)
    {
        if (!Enum.IsDefined(typeof(HelpStatus), status))
            return Errors.General.ValueIsInvalid();

        HelpStatus = (HelpStatus)status;

        return UnitResult.Success<Error>();
    }

    internal void AddPhoto(Photo photo)
    {
        _photos.Add(photo);
    }

    internal void RemovePhoto(Photo photo)
    {
        _photos.Remove(photo);
    }

    internal UnitResult<Error> MoveForward()
    {
        var newPosition = Position.Forward();
        if (newPosition.IsFailure)
            return newPosition.Error;

        Position = newPosition.Value;

        return Result.Success<Error>();
    }

    internal UnitResult<Error> MoveBack()
    {
        var newPosition = Position.Back();
        if (newPosition.IsFailure)
            return newPosition.Error;

        Position = newPosition.Value;

        return Result.Success<Error>();
    }

    public void SetSerialNumber(Position position) =>
        Position = position;

    public void Move(Position newPosition) =>
        Position = newPosition;
}