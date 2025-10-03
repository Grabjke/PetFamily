using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetFamily.Core;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;

namespace PetFamily.VolunteersApplications.Application.VolunteersApplications.Commands.Revision;

public class RevisionApplicationHandler : ICommandHandler<RevisionApplicationCommand>
{
    private readonly ILogger<RevisionApplicationHandler> _logger;
    private readonly IVolunteerApplicationsRepository _repository;
    private readonly IValidator<RevisionApplicationCommand> _validator;
    private readonly IUnitOfWork _unitOfWork;

    public RevisionApplicationHandler(
        ILogger<RevisionApplicationHandler> logger,
        IVolunteerApplicationsRepository repository,
        IValidator<RevisionApplicationCommand> validator,
        [FromKeyedServices(Modules.Application)]
        IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _repository = repository;
        _validator = validator;
        _unitOfWork = unitOfWork;
    }

    public async Task<UnitResult<ErrorList>> Handle(
        RevisionApplicationCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();

        var applicationResult = await _repository.GetById(command.ApplicationId, cancellationToken);
        if (applicationResult.IsSuccess == false)
            return applicationResult.Error.ToErrorList();

        var revisionResult = applicationResult.Value.Revision(command.AdminId, command.Comment);
        if (revisionResult.IsSuccess == false)
            return revisionResult.Error.ToErrorList();

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Application status {ApplicationId} has been updated", command.ApplicationId);

        return Result.Success<ErrorList>();
    }
}