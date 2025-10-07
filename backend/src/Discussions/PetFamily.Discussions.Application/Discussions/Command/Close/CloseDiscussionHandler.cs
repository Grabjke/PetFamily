using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetFamily.Core;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;

namespace PetFamily.Discussions.Application.Discussions.Command.Close;

public class CloseDiscussionHandler : ICommandHandler<Guid, CloseDiscussionCommand>
{
    private readonly IDiscussionRepository _repository;
    private readonly ILogger<CloseDiscussionHandler> _logger;
    private readonly IValidator<CloseDiscussionCommand> _validator;
    private readonly IUnitOfWork _unitOfWork;

    public CloseDiscussionHandler(
        IDiscussionRepository repository,
        ILogger<CloseDiscussionHandler> logger,
        IValidator<CloseDiscussionCommand> validator,
        [FromKeyedServices(Modules.Discussion)]
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _logger = logger;
        _validator = validator;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        CloseDiscussionCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();

        var discussionResult = await _repository.GetById(command.DiscussionId, cancellationToken);
        if (discussionResult.IsFailure)
            return discussionResult.Error.ToErrorList();

        var closeResult = discussionResult.Value.CloseDiscussion(command.AdminId);
        if (closeResult.IsFailure)
            return closeResult.Error.ToErrorList();

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Closing discussion {DiscussionId}", command.DiscussionId);

        return command.DiscussionId;
    }
}