using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Extensions;
using PetFamily.Application.FileProvider;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.Commands.DeletePet;

public class DeletePetHandler : ICommandHandler<Guid, DeletePetCommand>
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly IValidator<DeletePetCommand> _validator;
    private readonly ILogger<DeletePetHandler> _logger;
    private readonly IFileProvider _provider;

    public DeletePetHandler(
        IVolunteersRepository volunteersRepository,
        IValidator<DeletePetCommand> validator,
        ILogger<DeletePetHandler> logger,
        IFileProvider provider)
    {
        _volunteersRepository = volunteersRepository;
        _validator = validator;
        _logger = logger;
        _provider = provider;
    }


    public async Task<Result<Guid, ErrorList>> Handle(
        DeletePetCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();

        var volunteerResult = await _volunteersRepository
            .GetById(command.VolunteerId, cancellationToken);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error.ToErrorList();

        var pet = volunteerResult.Value.Pets
            .FirstOrDefault(p => p.Id.Value == command.PetId);
        if (pet is null)
            return Errors.General.NotFound(command.PetId).ToErrorList();

        var photoPaths = pet.Photos
            .Select(p => p.PathToStorage.Path)
            .ToList();

        var removeResult = await _provider.RemoveFiles(photoPaths, cancellationToken);
        if (removeResult.IsFailure)
            return removeResult.Error.ToErrorList();

        volunteerResult.Value.HardDeletePet(pet);
        
        await _volunteersRepository.Save(volunteerResult.Value, cancellationToken);
        
        _logger.LogInformation("Pet with id:{PetId} hard deleted", command.PetId);
        
        return pet.Id.Value;
    }
}