using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetFamily.Core;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.Discussions.Contracts;
using PetFamily.Discussions.Contracts.Requests;
using PetFamily.SharedKernel;

namespace PetFamily.VolunteersApplications.Application.VolunteersApplications.Commands.Take;

public class TakeApplicationHandler : ICommandHandler<Guid, TakeApplicationCommand>
{
    private readonly IValidator<TakeApplicationCommand> _validator;
    private readonly IVolunteerApplicationsRepository _repository;
    private readonly ILogger<TakeApplicationHandler> _logger;
    private readonly IDiscussionContract _contract;
    private readonly IUnitOfWork _unitOfWork;

    public TakeApplicationHandler(
        IValidator<TakeApplicationCommand> validator,
        IVolunteerApplicationsRepository repository,
        ILogger<TakeApplicationHandler> logger,
        IDiscussionContract contract,
        [FromKeyedServices(Modules.Application)]
        IUnitOfWork unitOfWork)
    {
        _validator = validator;
        _repository = repository;
        _logger = logger;
        _contract = contract;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        TakeApplicationCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
        {
            return validationResult.ToErrorList();
        }

        await _unitOfWork.BeginTransactionAsync(cancellationToken: cancellationToken);
        try
        {
            var applicationResult = await _repository.GetById(command.ApplicationId, cancellationToken);
            if (applicationResult.IsFailure)
                return applicationResult.Error.ToErrorList();

            var discussionIdResult = await _contract.AddDiscussion(
                new AddDiscussionRequest(
                    command.ApplicationId,
                    [command.AdminId, applicationResult.Value.UserId]),
                cancellationToken);

            if (discussionIdResult.IsFailure)
                return discussionIdResult.Error;

            applicationResult.Value.TakeToReview(command.AdminId, discussionIdResult.Value);

            await _unitOfWork.CommitAsync(cancellationToken);

            _logger.LogInformation(
                "Application id:{ApplicationId} has been processed and discussion id:{DiscussionId} has been initialized",
                command.ApplicationId, discussionIdResult.Value);
            return applicationResult.Value.Id;
        }
        catch
        {
            await _unitOfWork.RollbackAsync(cancellationToken);
            _logger.LogError("Error in accepting the application");
            return Errors.General.Failure().ToErrorList();
        }
    }
}