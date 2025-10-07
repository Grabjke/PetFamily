using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;

namespace PetFamily.Discussions.Application.Discussions.Command.DeleteMessage;

public class DeleteMessageHandler : ICommandHandler<DeleteMessageCommand>
{
    private readonly ILogger<DeleteMessageHandler> _logger;
    private readonly IValidator<DeleteMessageCommand> _validator;
    private readonly IDiscussionRepository _repository;

    public DeleteMessageHandler(
        ILogger<DeleteMessageHandler> logger,
        IValidator<DeleteMessageCommand> validator,
        IDiscussionRepository repository)
    {
        _logger = logger;
        _validator = validator;
        _repository = repository;
    }

    public async Task<UnitResult<ErrorList>> Handle(
        DeleteMessageCommand command,
        CancellationToken cancellationToken = default)
    {
        var validateResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validateResult.IsValid == false)
            return validateResult.ToErrorList();

        var discussionResult = await _repository.GetById(command.DiscussionId, cancellationToken);
        if (discussionResult.IsFailure)
            return discussionResult.Error.ToErrorList();

        var deleteResult = discussionResult.Value.RemoveMessage(command.MessageId, command.UserId);
        if (deleteResult.IsFailure)
            return deleteResult.Error.ToErrorList();

        await _repository.Save(discussionResult.Value, cancellationToken);

        _logger.LogInformation("Deleted message {MessageId}", command.MessageId);

        return Result.Success<ErrorList>();
    }
}