using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.Discussions.Domain.DiscussionManagement;
using PetFamily.SharedKernel;

namespace PetFamily.Discussions.Application.Discussions.Command.Create;

public class CreateDiscussionHandler : ICommandHandler<Guid, CreateDiscussionCommand>
{
    private readonly IValidator<CreateDiscussionCommand> _validator;
    private readonly IDiscussionRepository _repository;
    private readonly ILogger<CreateDiscussionHandler> _logger;

    public CreateDiscussionHandler(
        IValidator<CreateDiscussionCommand> validator,
        IDiscussionRepository repository,
        ILogger<CreateDiscussionHandler> logger)
    {
        _validator = validator;
        _repository = repository;
        _logger = logger;
    }


    public async Task<Result<Guid, ErrorList>> Handle(CreateDiscussionCommand command,
        CancellationToken cancellationToken = default)
    {
        var validateResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validateResult.IsValid == false)
            return validateResult.ToErrorList();

        var discussionResult = Discussion.Create(command.RelationId, command.UsersIds);
        if (discussionResult.IsFailure)
            return discussionResult.Error.ToErrorList();

        var discussionId = await _repository.AddDiscussion(discussionResult.Value, cancellationToken);
        
        _logger.LogInformation("Created discussion {DiscussionId}", discussionId);
        
        return discussionId;
    }
}