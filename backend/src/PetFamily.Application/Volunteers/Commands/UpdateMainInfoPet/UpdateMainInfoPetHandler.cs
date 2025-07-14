using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.ValueObjects.Breed;
using PetFamily.Domain.ValueObjects.Pet;
using PetFamily.Domain.ValueObjects.Species;
using PetFamily.Domain.ValueObjects.Volunteer;

namespace PetFamily.Application.Volunteers.Commands.UpdateMainInfoPet;

public class UpdateMainInfoPetHandler : ICommandHandler<Guid, UpdateMainInfoPetCommand>
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly IValidator<UpdateMainInfoPetCommand> _validator;
    private readonly ILogger<UpdateMainInfoPetHandler> _logger;
    private readonly IReadDbContext _readDbContext;


    public UpdateMainInfoPetHandler(
        IVolunteersRepository volunteersRepository,
        IValidator<UpdateMainInfoPetCommand> validator,
        ILogger<UpdateMainInfoPetHandler> logger,
        IReadDbContext readDbContext)
    {
        _volunteersRepository = volunteersRepository;
        _validator = validator;
        _logger = logger;
        _readDbContext = readDbContext;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        UpdateMainInfoPetCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();

        var speciesExist = await _readDbContext.Species
            .AnyAsync(s => s.Id == command.SpeciesId, cancellationToken: cancellationToken);
        if (!speciesExist)
            return Errors.General.NotFound(command.SpeciesId).ToErrorList();

        var breedBelongsToSpecies = await _readDbContext.Species
            .Where(s => s.Id == command.SpeciesId)
            .SelectMany(s => s.Breeds!)
            .AnyAsync(b => b.Id == command.BreedId, cancellationToken);
        if (!breedBelongsToSpecies)
            return Errors.General.NotFound(command.BreedId).ToErrorList();

        var petSpecies = SpeciesId.Create(command.SpeciesId);
        var petBreed = BreedId.Create(command.BreedId);

        var volunteerResult = await _volunteersRepository.GetById(command.VolunteerId, cancellationToken);
        if (volunteerResult.IsFailure)
            return Errors.General.NotFound(command.VolunteerId).ToErrorList();

        var pet = volunteerResult.Value.Pets.FirstOrDefault(p => p.Id.Value == command.PetId);
        if(pet is null)
            return Errors.General.NotFound(command.PetId).ToErrorList();
        
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
        var petWeight = PetWeight.Create(command.Weight).Value;
        var petHeight = PetHeight.Create(command.Height).Value;
        var ownersPhoneNumber = OwnersPhoneNumber.Create(command.OwnersPhoneNumber).Value;
        var castration = command.Castration;
        var birthday = Birthday.Create(command.Birthday).Value;
        var isVaccinated = command.isVaccinated;
        var helpStatus = command.HelpStatus;

        pet.UpdateMainInfo(
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

        await _volunteersRepository.Save(volunteerResult.Value, cancellationToken);

        _logger.LogInformation("Update pet with id:{petId} to volunteer with id: {volunteerId}",
            command.PetId, volunteerResult.Value.Id);

        return command.PetId;
    }
}