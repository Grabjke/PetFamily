using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Core.Extensions;
using PetFamily.VolunteersApplications.Infrastructure.Outbox;

namespace PetFamily.VolunteersApplications.Infrastructure.Configurations.Write;

public class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.HasKey(o => o.Id);

        builder.Property(o => o.Payload)
            .HasColumnType("jsonb")
            .IsRequired();

        builder.Property(o => o.OccurredOnUtc)
            .SetDefaultDateTimeKind(DateTimeKind.Utc)
            .IsRequired();

        builder.Property(o => o.ProcessedOnUtc)
            .SetDefaultDateTimeKind(DateTimeKind.Utc)
            .IsRequired(false);

        builder.Property(o => o.Type)
            .HasMaxLength(2000);

        builder.HasIndex(e => new
            {
                e.OccurredOnUtc,
                e.ProcessedOnUtc,
            })
            .HasDatabaseName("idx_outbox_messages_unprocessed")
            .IncludeProperties(e => new
            {
                e.Id,
                e.Type,
                e.Payload,
            })
            .HasFilter("processed_on_utc IS NULL");
    }
}