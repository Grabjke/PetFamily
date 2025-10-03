using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Core.Extensions;
using PetFamily.Discussions.Domain.DiscussionManagement;
using PetFamily.SharedKernel;

namespace PetFamily.Discussions.Infrastructure.Configurations.Write;

public class MessageConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.ToTable("messages");
        
        builder.HasKey(x => x.Id);

        builder.ComplexProperty(m => m.Text, t =>
        {
            t.Property(t => t.Value)
                .HasMaxLength(Constants.MAX_HIGH_TEXT_LENGTH)
                .IsRequired()
                .HasColumnName("text");
        });

        builder.Property(m => m.CreatedAt)
            .IsRequired()
            .SetDefaultDateTimeKind(DateTimeKind.Utc);

        builder.Property(m => m.IsEdited)
            .IsRequired()
            .HasColumnName("is_edited")
            .HasDefaultValue(false);

        builder.Property(m => m.UserId)
            .HasColumnName("user_id")
            .IsRequired();
    }
}