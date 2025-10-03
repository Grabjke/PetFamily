using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;
using PetFamily.VolunteersApplications.Domain.ApplicationManagement;

namespace PetFamily.VolunteersApplications.Infrastructure.Configurations.Write;

public class VolunteerApplicationConfiguration : IEntityTypeConfiguration<VolunteerApplication>
{
    public void Configure(EntityTypeBuilder<VolunteerApplication> builder)
    {
        builder.ToTable("applications");

        builder.HasKey(x => x.Id);

        builder.Property(v => v.UserId)
            .HasColumnName("user_id")
            .IsRequired();

        builder.ComplexProperty(v => v.VolunteerInfo, vi =>
        {
            vi.Property(vi => vi.Description)
                .HasColumnName("description")
                .HasMaxLength(Constants.MAX_HIGH_TEXT_LENGTH);

            vi.Property(vi => vi.PhoneNumber)
                .HasColumnName("phone_number")
                .HasMaxLength(Constants.PHONE_NUMBER_LENGTH)
                .IsRequired();

            vi.Property(vi => vi.FirstName)
                .HasColumnName("firstname")
                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH)
                .IsRequired();

            vi.Property(vi => vi.LastName)
                .HasColumnName("lastname")
                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH)
                .IsRequired();

            vi.Property(vi => vi.Surname)
                .HasColumnName("surname")
                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);

            vi.Property(vi => vi.Email)
                .HasColumnName("email")
                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);
        });

        builder.Property(v => v.AdminId)
            .HasColumnName("admin_id");

        builder.Property(v => v.DiscussionId)
            .HasColumnName("discussion_id");

        builder.Property(v => v.Status)
            .HasColumnName("status")
            .HasConversion<string>()
            .IsRequired();

        builder.Property(v => v.Comment)
            .HasColumnName("comment")
            .HasMaxLength(Constants.MAX_HIGH_TEXT_LENGTH);

        builder.Property(v => v.CreatedAt)
            .HasColumnName("created_at")
            .SetDefaultDateTimeKind(DateTimeKind.Utc);

        builder.Property(v => v.UpdatedAt)
            .HasColumnName("updated_at")
            .SetDefaultDateTimeKind(DateTimeKind.Utc);
    }
}