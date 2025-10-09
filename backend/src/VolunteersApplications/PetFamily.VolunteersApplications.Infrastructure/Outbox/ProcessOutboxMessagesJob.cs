using Quartz;

namespace PetFamily.VolunteersApplications.Infrastructure.Outbox;

[DisallowConcurrentExecution]
public class ProcessOutboxMessagesJob : IJob
{
    private readonly ProcessedOutboxMessagesService _processedOutboxMessagesService;

    public ProcessOutboxMessagesJob(ProcessedOutboxMessagesService processedOutboxMessagesService)
    {
        _processedOutboxMessagesService = processedOutboxMessagesService;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        await _processedOutboxMessagesService.Execute(context.CancellationToken);
    }
}