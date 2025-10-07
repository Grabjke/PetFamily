using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Core.Dtos.Query;
using PetFamily.Core.Extensions;

namespace PetFamily.Discussions.Infrastructure.Configurations.Read;

public class DiscussionDtoConfiguration : IEntityTypeConfiguration<DiscussionDto>
{
    public void Configure(EntityTypeBuilder<DiscussionDto> builder)
    {
        builder.ToTable("discussions");

        builder.HasKey(d => d.Id);

        builder.Property(d => d.RelationId)
            .HasColumnName("relation_id");

        builder.Property(d => d.UsersIds)
            .HasUuidArrayConversion()
            .HasColumnName("user_ids");

        builder.Property(d => d.Status)
            .HasColumnName("status")
            .HasConversion<string>();
        
        builder.HasMany(d => d.Messages)
            .WithOne()
            .HasForeignKey(d => d.DiscussionId);
    }
}