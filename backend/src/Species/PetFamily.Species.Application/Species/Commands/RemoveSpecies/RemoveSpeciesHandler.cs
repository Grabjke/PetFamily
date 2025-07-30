using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetFamily.Core;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;
using PetFamily.Volunteers.Contracts;
using PetFamily.Volunteers.Contracts.Requests;

namespace PetFamily.Species.Application.Species.Commands.RemoveSpecies;

public class RemoveSpeciesHandler : ICommandHandler<Guid, RemoveSpeciesCommand>
{
    private readonly ILogger<RemoveSpeciesHandler> _logger;
    private readonly ISpeciesRepository _speciesRepository;
    private readonly ISpeciesReadDbContext _readDbContext;
    private readonly IValidator<RemoveSpeciesCommand> _validator;
    private readonly IVolunteerContract _volunteerContract;
    private readonly IUnitOfWork _unitOfWork;

    public RemoveSpeciesHandler(
        ILogger<RemoveSpeciesHandler> logger,
        ISpeciesRepository speciesRepository,
        ISpeciesReadDbContext readDbContext,
        IValidator<RemoveSpeciesCommand> validator,
        IVolunteerContract volunteerContract,
        [FromKeyedServices(Modules.Species)]IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _speciesRepository = speciesRepository;
        _readDbContext = readDbContext;
        _validator = validator;
        _volunteerContract = volunteerContract;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        RemoveSpeciesCommand command,
        CancellationToken cancellationToken = default)
    {
        var commandResult = await _validator.ValidateAsync(command, cancellationToken);
        if (commandResult.IsValid == false)
            return commandResult.ToErrorList();
        
        var hasAnimalsWithSpecies = await _volunteerContract.HasAnimalsWithSpecies(
                new HasAnimalsWithSpeciesRequest(command.SpeciesId),
                cancellationToken);
        
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