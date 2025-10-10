using System.Text.Json;
using PetFamily.VolunteersApplications.Application;
using PetFamily.VolunteersApplications.Infrastructure.DbContexts;
using PetFamily.VolunteersApplications.Infrastructure.Outbox;

namespace PetFamily.VolunteersApplications.Infrastructure.Repositories;

public class OutBoxRepository : IOutBoxRepository
{
    private readonly WriteApplicationDbContext _context;

    public OutBoxRepository(WriteApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Add<T>(T message, CancellationToken cancellationToken)
    {
        var outboxMessage = new OutboxMessage
        {
            Id = Guid.NewGuid(),
            OccurredOnUtc = DateTime.UtcNow,
            Type = typeof(T).FullName!,
            Payload = JsonSerializer.Serialize(message)
        };

        await _context.OutboxMessages.AddAsync(outboxMessage, cancellationToken);
    }
}