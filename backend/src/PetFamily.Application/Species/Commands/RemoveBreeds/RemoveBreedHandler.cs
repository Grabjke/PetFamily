using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.ValueObjects.Breed;

namespace PetFamily.Application.Species.Commands.RemoveBreeds;

public class RemoveBreedHandler : ICommandHandler<Guid, RemoveBreedCommand>
{
    private readonly ILogger<RemoveBreedHandler> _logger;
    private readonly ISpeciesRepository _speciesRepository;
    private readonly IReadDbContext _readDbContext;
    private readonly IValidator<RemoveBreedCommand> _validator;
    private readonly IUnitOfWork _unitOfWork;

    public RemoveBreedHandler(
        ILogger<RemoveBreedHandler> logger,
        ISpeciesRepository speciesRepository,
        IReadDbContext readDbContext,
        IValidator<RemoveBreedCommand> validator,
        IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _speciesRepository = speciesRepository;
        _readDbContext = readDbContext;
        _validator = validator;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        RemoveBreedCommand command,
        CancellationToken cancellationToken = default)
    {
        var commandResult = await _validator.ValidateAsync(command, cancellationToken);
        if (commandResult.IsValid == false)
            return commandResult.ToErrorList();

        var hasAnimalsWithBreed = await _readDbContext.Pets
            .AnyAsync(p => p.BreedId == command.BreedId, cancellationToken);
        if (hasAnimalsWithBreed)
            return Errors.Breed.CannotDeleteBecauseHasAnimals().ToErrorList();

        var speciesResult = await _speciesRepository.GetById(command.SpeciesId, cancellationToken);
        if (speciesResult.IsFailure)
            return speciesResult.Error.ToErrorList();

        var breedId = BreedId.Create(command.BreedId);

        var breed = speciesResult.Value.Breeds.FirstOrDefault(b => b.Id == breedId);
        if (breed is null)
            return Errors.General.NotFound().ToErrorList();

        speciesResult.Value.DeleteBreed(breed);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("Successfully removed breed with id:{BreedId}",
            command.BreedId);

        return speciesResult.Value.Id.Value;
    }
}