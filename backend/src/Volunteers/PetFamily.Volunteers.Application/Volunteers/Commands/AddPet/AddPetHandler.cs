using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects;
using PetFamily.Species.Contracts;
using PetFamily.Species.Contracts.Requests;
using PetFamily.Volunteers.Domain.PetManagement;
using PetFamily.Volunteers.Domain.PetManagement.ValueObjects.Pet;
using PetFamily.Volunteers.Domain.PetManagement.ValueObjects.Volunteer;

namespace PetFamily.Volunteers.Application.Volunteers.Commands.AddPet;

public class AddPetHandler : ICommandHandler<Guid, AddPetCommand>
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly IValidator<AddPetCommand> _validator;
    private readonly ILogger<AddPetHandler> _logger;
    private readonly ISpeciesContract _speciesContract;

    public AddPetHandler(
        IVolunteersRepository volunteersRepository,
        IValidator<AddPetCommand> validator,
        ILogger<AddPetHandler> logger,
        ISpeciesContract speciesContract)
    {
        _volunteersRepository = volunteersRepository;
        _validator = validator;
        _logger = logger;
        _speciesContract = speciesContract;
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

        var isSpeciesAndBreedExist = await _speciesContract.IsSpeciesAndBreedExist(
            new IsSpeciesAndBreedExistRequest(command.SpeciesId, command.BreedId), cancellationToken);
        if (!isSpeciesAndBreedExist)
            return Errors.Species.SpeciesAndBreedExist().ToErrorList();
        
        var petSpecies = SpeciesId.Create(command.SpeciesId);

        var petBreed = BreedId.Create(command.BreedId);

        var volunteerResult = await _volunteersRepository
            .GetById(command.VolunteerId, cancellationToken);

        if (volunteerResult.IsFailure)
            return volunteerResult.Error.ToErrorList();

        var petId = PetId.NewPetId();
        var petName = PetName.Create(command.Name).Value;
        var petDescription = Description.Create(command.Description).Value;
        var petSpeciesBreed = PetSpeciesBreed.Create(petSpecies, petBreed).Value;
        var petColour = Colour.Create(command.Colour).Value;
        var petHealthInformation = HealthInformation.Create(command.HealthInformation).Value;
        var address = Address.Create(
            command.Address.Street,
            command.Address.City,
            command.Address.Country,
            command.Address.ZipCode).Value;
        var petWeight = Weight.Create(command.Weight).Value;
        var petHeight = Height.Create(command.Height).Value;
        var ownersPhoneNumber = OwnersPhoneNumber.Create(command.OwnersPhoneNumber).Value;
        var castration = command.Castration;
        var birthday = Birthday.Create(command.Birthday).Value;
        var isVaccinated = command.isVaccinated;
        var helpStatus = command.HelpStatus;

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

        var addResult = volunteerResult.Value.AddPet(pet);
        if (addResult.IsFailure)
            return addResult.Error.ToErrorList();

        await _volunteersRepository.Save(volunteerResult.Value, cancellationToken);

        _logger.LogInformation("Added pet with id:{petId} to volunteer with id: {volunteerId}",
            petId, volunteerResult.Value.Id);

        return petId.Value;
    }
}