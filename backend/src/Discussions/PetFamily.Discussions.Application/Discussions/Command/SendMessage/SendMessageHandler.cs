using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetFamily.Core;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.Core.ValueObjects.Discussion;
using PetFamily.Discussions.Domain.DiscussionManagement;
using PetFamily.SharedKernel;

namespace PetFamily.Discussions.Application.Discussions.Command.SendMessage;

public class SendMessageHandler : ICommandHandler<Guid, SendMessageCommand>
{
    private readonly ILogger<SendMessageHandler> _logger;
    private readonly IDiscussionRepository _repository;
    private readonly IValidator<SendMessageCommand> _validator;

    public SendMessageHandler(
        ILogger<SendMessageHandler> logger,
        IDiscussionRepository repository,
        IValidator<SendMessageCommand> validator)
    {
        _logger = logger;
        _repository = repository;
        _validator = validator;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        SendMessageCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();

        var discussionResult = await _repository.GetById(command.DiscussionId, cancellationToken);
        if (discussionResult.IsFailure)
            return discussionResult.Error.ToErrorList();
        
        var message = Message.Create(Text.Create(command.Message).Value, command.UserId).Value;

        discussionResult.Value.AddMessage(message);

        await _repository.Save(discussionResult.Value, cancellationToken);

        _logger.LogInformation("Added message id:{id}", message.Id);

        return message.Id;
    }
}