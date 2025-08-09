using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetFamily.Core;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects;
using PetFamily.Volunteers.Contracts;
using PetFamily.Volunteers.Contracts.Requests;

namespace PetFamily.Species.Application.Species.Commands.RemoveBreeds;

public class RemoveBreedHandler : ICommandHandler<Guid, RemoveBreedCommand>
{
    private readonly ILogger<RemoveBreedHandler> _logger;
    private readonly ISpeciesRepository _speciesRepository;
    private readonly IValidator<RemoveBreedCommand> _validator;
    private readonly IVolunteerContract _volunteerContract;
    private readonly IUnitOfWork _unitOfWork;

    public RemoveBreedHandler(
        ILogger<RemoveBreedHandler> logger,
        ISpeciesRepository speciesRepository,
        ISpeciesReadDbContext readDbContext,
        IValidator<RemoveBreedCommand> validator,
        IVolunteerContract volunteerContract,
        [FromKeyedServices(Modules.Species)]IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _speciesRepository = speciesRepository;
        _validator = validator;
        _volunteerContract = volunteerContract;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        RemoveBreedCommand command,
        CancellationToken cancellationToken = default)
    {
        var commandResult = await _validator.ValidateAsync(command, cancellationToken);
        if (commandResult.IsValid == false)
            return commandResult.ToErrorList();

        var hasAnimalsWithBreed = await _volunteerContract.HasAnimalsWithBreed(
            new HasAnimalsWithBreedRequest(command.BreedId),cancellationToken);
        if (hasAnimalsWithBreed)
            return Errors.Breed.CannotDeleteBecauseHasAnimals().ToErrorList();

        var speciesResult = await _speciesRepository.GetById(command.SpeciesId, cancellationToken);
        if (speciesResult.IsFailure)
            return speciesResult.Error.ToErrorList();

        var breedId = BreedId.Create(command.BreedId);

        var breed = speciesResult.Value.Breeds
            .FirstOrDefault(b => b.Id == breedId);
        if (breed is null)
            return Errors.General.NotFound().ToErrorList();

        speciesResult.Value.DeleteBreed(breed);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("Successfully removed breed with id:{BreedId}",
            command.BreedId);

        return speciesResult.Value.Id.Value;
    }
}