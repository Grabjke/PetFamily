using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Extensions;
using PetFamily.Application.Species;
using PetFamily.Domain.Entities;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Species;
using PetFamily.Domain.ValueObjects.Breed;
using PetFamily.Domain.ValueObjects.Pet;
using PetFamily.Domain.ValueObjects.Species;
using PetFamily.Domain.ValueObjects.Volunteer;


namespace PetFamily.Application.Volunteers.AddPets;

public class AddPetHandler
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly ISpeciesRepository _speciesRepository;
    private readonly IValidator<AddPetCommand> _validator;
    private readonly ILogger<AddPetHandler> _logger;

    public AddPetHandler(
        IVolunteersRepository volunteersRepository,
        ISpeciesRepository speciesRepository,
        IValidator<AddPetCommand> validator,
        ILogger<AddPetHandler> logger)
    {
        _volunteersRepository = volunteersRepository;
        _speciesRepository = speciesRepository;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        AddPetCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
        {
            return validationResult.ToErrorList();
        }

        var petSpecies = SpeciesId.Create(command.SpeciesId);
        var petBreed = BreedId.Create(command.BreedId);
        
        var resultVerify = await _speciesRepository.VerifySpeciesAndBreedExist(
            petSpecies,
            petBreed,
            cancellationToken);
       
        if(resultVerify.IsFailure)
            return resultVerify.Error.ToErrorList();
        
        var volunteerResult = await _volunteersRepository
            .GetById(command.VolunteerId, cancellationToken);
        
        if(volunteerResult.IsFailure)
            return volunteerResult.Error.ToErrorList();

        var petId = PetId.NewPetId();
        var petName = PetName.Create(command.Name).Value;
        var petDescription = PetDescription.Create(command.Description).Value;
        var petSpeciesBreed = PetSpeciesBreed.Create(petSpecies, petBreed).Value;
        var petColour = PetColour.Create(command.Colour).Value;
        var petHealthInformation = PetHealthInformation.Create(command.HealthInformation).Value;
        var address = Address.Create(
            command.Address.Street,
            command.Address.City,
            command.Address.Country,
            command.Address.ZipCode).Value;
        var petWeight=PetWeight.Create(command.Weight).Value;
        var petHeight = PetHeight.Create(command.Height).Value;
        var ownersPhoneNumber=OwnersPhoneNumber.Create(command.OwnersPhoneNumber).Value;   
        var castration=command.Castration;
        var birthday=Birthday.Create(command.Birthday).Value;
        var isVaccinated = command.isVaccinated;
        var helpStatus=command.HelpStatus;

        var pet = new Pet(
            petId,
            petName,
            petDescription,
            petSpeciesBreed,
            petColour,
            petHealthInformation,
            address,
            petWeight,
            petHeight,
            ownersPhoneNumber,
            castration,
            birthday,
            isVaccinated,
            helpStatus);
        
        volunteerResult.Value.AddPet(pet);

        await _volunteersRepository.Save(volunteerResult.Value, cancellationToken);
        
        _logger.LogInformation("Added pet with id:{petId} to volunteer with id: {volunteerId}",
            petId,volunteerResult.Value.Id);
        
        return petId.Value;
    }
}