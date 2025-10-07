using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Core.Dtos.Query;

namespace PetFamily.Discussions.Infrastructure.Configurations.Read;

public class MessageDtoConfiguration : IEntityTypeConfiguration<MessageDto>
{
    public void Configure(EntityTypeBuilder<MessageDto> builder)
    {
        builder.ToTable("messages");

        builder.HasKey(m => m.Id);

        builder.Property(m => m.IsEdited)
            .HasColumnName("is_edited");

        builder.Property(m => m.UserId)
            .HasColumnName("user_id");

        builder.Property(m => m.Text)
            .HasColumnName("text");

        builder.Property(m => m.CreatedAt)
            .HasColumnName("created_at");

        builder.Property(m => m.DiscussionId)
            .HasColumnName("discussion_id");
    }
}