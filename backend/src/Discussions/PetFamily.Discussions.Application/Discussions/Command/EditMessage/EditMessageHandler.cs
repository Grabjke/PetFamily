using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.Core.ValueObjects.Discussion;
using PetFamily.SharedKernel;

namespace PetFamily.Discussions.Application.Discussions.Command.EditMessage;

public class EditMessageHandler : ICommandHandler<EditMessageCommand>
{
    private readonly ILogger<EditMessageHandler> _logger;
    private readonly IDiscussionRepository _repository;
    private readonly IValidator<EditMessageCommand> _validator;

    public EditMessageHandler(
        ILogger<EditMessageHandler> logger,
        IDiscussionRepository repository,
        IValidator<EditMessageCommand> validator)
    {
        _logger = logger;
        _repository = repository;
        _validator = validator;
    }

    public async Task<UnitResult<ErrorList>> Handle(
        EditMessageCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();

        var discussionResult = await _repository.GetById(command.DiscussionId, cancellationToken);
        if (discussionResult.IsFailure)
            return discussionResult.Error.ToErrorList();

        var editResult = discussionResult.Value.EditMessage(
            Text.Create(command.Message).Value,
            command.MessageId,
            command.UserId);

        if (editResult.IsFailure)
            return editResult.Error.ToErrorList();

        await _repository.Save(discussionResult.Value, cancellationToken);

        _logger.LogInformation("Successfully edited message id:{id}", command.MessageId);

        return Result.Success<ErrorList>();
    }
}