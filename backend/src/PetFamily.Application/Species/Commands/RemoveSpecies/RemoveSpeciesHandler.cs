using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Species.Commands.RemoveSpecies;

public class RemoveSpeciesHandler : ICommandHandler<Guid, RemoveSpeciesCommand>
{
    private readonly ILogger<RemoveSpeciesHandler> _logger;
    private readonly ISpeciesRepository _speciesRepository;
    private readonly IReadDbContext _readDbContext;
    private readonly IValidator<RemoveSpeciesCommand> _validator;
    private readonly IUnitOfWork _unitOfWork;

    public RemoveSpeciesHandler(
        ILogger<RemoveSpeciesHandler> logger,
        ISpeciesRepository speciesRepository,
        IReadDbContext readDbContext,
        IValidator<RemoveSpeciesCommand> validator,
        IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _speciesRepository = speciesRepository;
        _readDbContext = readDbContext;
        _validator = validator;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        RemoveSpeciesCommand command,
        CancellationToken cancellationToken = default)
    {
        var commandResult = await _validator.ValidateAsync(command, cancellationToken);
        if (commandResult.IsValid == false)
            return commandResult.ToErrorList();
        
        var hasAnimalsWithSpecies = await _readDbContext.Pets
            .AnyAsync(p => p.SpeciesId == command.SpeciesId, cancellationToken);
        if (hasAnimalsWithSpecies)
            return Errors.Species.CannotDeleteBecauseHasAnimals().ToErrorList();

        var speciesResult = await _speciesRepository.GetById(command.SpeciesId, cancellationToken);
        if (speciesResult.IsFailure)
            return speciesResult.Error.ToErrorList();
        
        await _speciesRepository.DeleteSpecies(speciesResult.Value, cancellationToken);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Successfully removed species with id:{SpeciesId}",
            command.SpeciesId);

        return speciesResult.Value.Id.Value;
    }
}