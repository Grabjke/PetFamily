using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetFamily.Accounts.Contracts;
using PetFamily.Accounts.Contracts.Requests;
using PetFamily.Core;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;

namespace PetFamily.VolunteersApplications.Application.VolunteersApplications.Commands.Reject;

public class RejectApplicationHandler : ICommandHandler<RejectApplicationCommand>
{
    private readonly ILogger<RejectApplicationHandler> _logger;
    private readonly IVolunteerApplicationsRepository _repository;
    private readonly IValidator<RejectApplicationCommand> _validator;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAccountContract _contract;

    public RejectApplicationHandler(
        ILogger<RejectApplicationHandler> logger,
        IVolunteerApplicationsRepository repository,
        IValidator<RejectApplicationCommand> validator,
        [FromKeyedServices(Modules.Application)]
        IUnitOfWork unitOfWork,
        IAccountContract contract)
    {
        _logger = logger;
        _repository = repository;
        _validator = validator;
        _unitOfWork = unitOfWork;
        _contract = contract;
    }

    public async Task<UnitResult<ErrorList>> Handle(RejectApplicationCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();

        var applicationResult = await _repository.GetById(command.ApplicationId, cancellationToken);
        if (applicationResult.IsSuccess == false)
            return applicationResult.Error.ToErrorList();

        var revisionResult = applicationResult.Value.Reject(command.AdminId, command.Comment);
        if (revisionResult.IsSuccess == false)
            return revisionResult.Error.ToErrorList();

        var bannedUserResult = await _contract.BannedUserApplication(
            new BannedUserApplicationRequest(applicationResult.Value.UserId), cancellationToken);
        
        if (bannedUserResult.IsSuccess == false)
            return bannedUserResult.Error.ToErrorList();

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Application status {ApplicationId} has been updated", command.ApplicationId);

        return Result.Success<ErrorList>();
    }
}