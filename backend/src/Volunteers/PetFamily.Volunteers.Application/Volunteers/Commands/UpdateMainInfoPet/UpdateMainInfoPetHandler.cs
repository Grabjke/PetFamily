using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFamily.Core;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects;
using PetFamily.Species.Contracts;
using PetFamily.Species.Contracts.Requests;
using PetFamily.Volunteers.Domain.PetManagement.ValueObjects;
using PetFamily.Volunteers.Domain.PetManagement.ValueObjects.Pet;
using PetFamily.Volunteers.Domain.PetManagement.ValueObjects.Volunteer;

namespace PetFamily.Volunteers.Application.Volunteers.Commands.UpdateMainInfoPet;

public class UpdateMainInfoPetHandler : ICommandHandler<Guid, UpdateMainInfoPetCommand>
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly IValidator<UpdateMainInfoPetCommand> _validator;
    private readonly ILogger<UpdateMainInfoPetHandler> _logger;
    private readonly ISpeciesContract _speciesContract;


    public UpdateMainInfoPetHandler(
        IVolunteersRepository volunteersRepository,
        IValidator<UpdateMainInfoPetCommand> validator,
        ILogger<UpdateMainInfoPetHandler> logger,
        ISpeciesContract speciesContract)
    {
        _volunteersRepository = volunteersRepository;
        _validator = validator;
        _logger = logger;
        _speciesContract = speciesContract;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        UpdateMainInfoPetCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();

        var isSpeciesAndBreedExist = await _speciesContract.IsSpeciesAndBreedExist(
            new IsSpeciesAndBreedExistRequest(command.SpeciesId, command.BreedId), cancellationToken);
        if (!isSpeciesAndBreedExist)
            return Errors.Species.SpeciesOrBreedNotExist().ToErrorList();

        var petSpecies = SpeciesId.Create(command.SpeciesId);
        var petBreed = BreedId.Create(command.BreedId);

        var volunteerResult = await _volunteersRepository.GetById(command.VolunteerId, cancellationToken);
        if (volunteerResult.IsFailure)
            return Errors.General.NotFound(command.VolunteerId).ToErrorList();

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

        var updateResult = volunteerResult.Value.UpdateMainInfoPet(
            PetId.Create(command.PetId),
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
        if (updateResult.IsFailure)
            return updateResult.Error.ToErrorList();

        await _volunteersRepository.Save(volunteerResult.Value, cancellationToken);

        _logger.LogInformation("Update pet with id:{petId} to volunteer with id: {volunteerId}",
            command.PetId, volunteerResult.Value.Id);

        return command.PetId;
    }
}