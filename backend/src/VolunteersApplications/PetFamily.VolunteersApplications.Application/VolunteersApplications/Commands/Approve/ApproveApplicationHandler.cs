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

namespace PetFamily.VolunteersApplications.Application.VolunteersApplications.Commands.Approve;

public class ApproveApplicationHandler : ICommandHandler<Guid, ApproveApplicationCommand>
{
    private readonly ILogger<ApproveApplicationHandler> _logger;
    private readonly IVolunteerApplicationsRepository _repository;
    private readonly IValidator<ApproveApplicationCommand> _validator;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAccountContract _contract;

    public ApproveApplicationHandler(
        ILogger<ApproveApplicationHandler> logger,
        IVolunteerApplicationsRepository repository,
        IValidator<ApproveApplicationCommand> validator,
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

    public async Task<Result<Guid, ErrorList>> Handle(
        ApproveApplicationCommand command,
        CancellationToken cancellationToken = default)
    {
        var validateResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validateResult.IsValid == false)
            return validateResult.ToErrorList();

        await _unitOfWork.BeginTransactionAsync(cancellationToken: cancellationToken);
        try
        {
            var applicationResult = await _repository.GetById(command.ApplicationId, cancellationToken);
            if (applicationResult.IsSuccess == false)
                return applicationResult.Error.ToErrorList();

            var createVolunteerResult = await _contract.CreateVolunteer(
                new CreateVolunteerRequest(applicationResult.Value.UserId), cancellationToken);
            if (createVolunteerResult.IsSuccess == false)
                return createVolunteerResult.Error.ToErrorList();

            var resultApprove = applicationResult.Value.Approve(command.AdminId);
            if (resultApprove.IsSuccess == false)
                return resultApprove.Error.ToErrorList();

            _logger.LogInformation("Approved application id:{id}.", command.ApplicationId);

            await _unitOfWork.CommitAsync(cancellationToken: cancellationToken);

            return command.ApplicationId;
        }
        catch (Exception e)
        {
            _logger.LogError("Failed approve application id:{id}.", command.ApplicationId);
            await _unitOfWork.RollbackAsync(cancellationToken);
            return Errors.General.Failure().ToErrorList();
        }
    }
}