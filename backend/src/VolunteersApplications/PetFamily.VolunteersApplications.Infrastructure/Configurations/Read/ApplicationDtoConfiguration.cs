using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Core.Dtos.Query;

namespace PetFamily.VolunteersApplications.Infrastructure.Configurations.Read;

public class ApplicationDtoConfiguration : IEntityTypeConfiguration<ApplicationDto>
{
    public void Configure(EntityTypeBuilder<ApplicationDto> builder)
    {
        builder.ToTable("applications");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.UserId)
            .HasColumnName("user_id");

        builder.Property(x => x.AdminId)
            .HasColumnName("admin_id");

        builder.Property(x => x.DiscussionId)
            .HasColumnName("discussion_id");

        builder.Property(x => x.Status)
            .HasConversion<string>()
            .HasColumnName("status");

        builder.Property(x => x.Comment)
            .HasColumnName("comment");

        builder.Property(x => x.CreatedAt)
            .HasColumnName("created_at");

        builder.Property(x => x.UpdatedAt)
            .HasColumnName("updated_at");

        builder.Property(x => x.Description)
            .HasColumnName("description");

        builder.Property(x => x.Email)
            .HasColumnName("email");

        builder.Property(x => x.FirstName)
            .HasColumnName("firstname");

        builder.Property(x => x.LastName)
            .HasColumnName("lastname");

        builder.Property(x => x.PhoneNumber)
            .HasColumnName("phone_number");

        builder.Property(x => x.Surname)
            .HasColumnName("surname");
    }
}