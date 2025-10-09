using System.Text.Json;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFamily.VolunteersApplications.Contracts;
using PetFamily.VolunteersApplications.Infrastructure.DbContexts;
using Polly;
using Polly.Retry;

namespace PetFamily.VolunteersApplications.Infrastructure.Outbox;

public class ProcessedOutboxMessagesService
{
    private readonly ILogger<ProcessedOutboxMessagesService> _logger;
    private readonly WriteApplicationDbContext _context;
    private readonly IPublishEndpoint _publisherEndpoint;

    public ProcessedOutboxMessagesService(
        ILogger<ProcessedOutboxMessagesService> logger,
        WriteApplicationDbContext context,
        IPublishEndpoint publisherEndpoint)
    {
        _logger = logger;
        _context = context;
        _publisherEndpoint = publisherEndpoint;
    }

    public async Task Execute(CancellationToken cancellationToken)
    {
        var messages = await _context.OutboxMessages
            .Where(m => m.ProcessedOnUtc == null)
            .OrderBy(m => m.OccurredOnUtc)
            .Take(100)
            .ToListAsync(cancellationToken: cancellationToken);

        if (messages.Count == 0)
            return;

        var pipeline = new ResiliencePipelineBuilder()
            .AddRetry(new RetryStrategyOptions
            {
                MaxRetryAttempts = 3,
                BackoffType = DelayBackoffType.Exponential,
                Delay = TimeSpan.FromSeconds(1),
                ShouldHandle = new PredicateBuilder().Handle<Exception>(),
                OnRetry = retryArguments =>
                {
                    _logger.LogCritical(retryArguments.Outcome.Exception, "Current attempt {attemptNumber}",
                        retryArguments.AttemptNumber);

                    return ValueTask.CompletedTask;
                }
            })
            .Build();

        var processingTasks =
            messages.Select(message => ProcessMessageAsync(message, pipeline, cancellationToken));

        await Task.WhenAll(processingTasks);

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error saving messages");
        }
    }


    private async Task ProcessMessageAsync(
        OutboxMessage outboxMessage,
        ResiliencePipeline pipeline,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        try

        {
            var messageType = AssemblyReference.Assembly.GetType(outboxMessage.Type)
                              ?? throw new NullReferenceException("Message type not found");

            var deserializedMessage = JsonSerializer.Deserialize(outboxMessage.Payload, messageType)
                                      ?? throw new NullReferenceException("Message payload not found");

            await pipeline.ExecuteAsync(async token =>
            {
                await _publisherEndpoint.Publish(deserializedMessage, messageType, token);

                outboxMessage.ProcessedOnUtc = DateTime.UtcNow;
            }, cancellationToken);
        }
        catch (Exception ex)
        {
            outboxMessage.Error = ex.Message;
            outboxMessage.ProcessedOnUtc = DateTime.UtcNow;
            _logger.LogError(ex, "Failed to process message ID: {MessageId}", outboxMessage.Id);
        }
    }
}