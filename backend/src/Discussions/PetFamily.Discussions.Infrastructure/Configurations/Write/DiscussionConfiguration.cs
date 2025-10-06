using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.VisualBasic;
using PetFamily.Core.Extensions;
using PetFamily.Discussions.Domain.DiscussionManagement;

namespace PetFamily.Discussions.Infrastructure.Configurations.Write;

public class DiscussionConfiguration : IEntityTypeConfiguration<Discussion>
{
    public void Configure(EntityTypeBuilder<Discussion> builder)
    {
        builder.ToTable("discussions");
        
        builder.HasKey(x => x.Id);

        builder.Property(x => x.RelationId)
            .IsRequired()
            .HasColumnName("relation_id");

        builder.Property(d => d.UsersIds)
            .HasColumnName("user_ids")
            .HasUuidArrayConversion()
            .IsRequired();

        builder.Property(d => d.Status)
            .HasColumnName("status")
            .HasConversion<string>();

        builder.HasMany(d => d.Messages)
            .WithOne()
            .HasForeignKey("discussion_id")
            .IsRequired();
    }
}