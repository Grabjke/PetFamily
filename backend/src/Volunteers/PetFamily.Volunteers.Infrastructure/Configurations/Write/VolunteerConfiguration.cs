using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects;
using PetFamily.Volunteers.Domain.PetManagement;

namespace PetFamily.Volunteers.Infrastructure.Configurations.Write;

public class VolunteerConfiguration : IEntityTypeConfiguration<Volunteer>
{
    public void Configure(EntityTypeBuilder<Volunteer> builder)
    {
        builder.ToTable("volunteers");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasConversion(
                id => id.Value,
                value => VolunteerId.Create(value));

        builder.ComplexProperty(v => v.FullName, fnb =>
        {
            fnb.Property(fn => fn.Name)
                .IsRequired()
                .HasColumnName("petName")
                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);

            fnb.Property(fn => fn.Surname)
                .IsRequired()
                .HasColumnName("surname")
                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);

            fnb.Property(fn => fn.Patronymic)
                .IsRequired(false)
                .HasColumnName("patronymic")
                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);
        });

        builder.ComplexProperty(v => v.Email, enb =>
        {
            enb.Property(e => e.Value)
                .IsRequired()
                .HasColumnName("email")
                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);
        });


        builder.ComplexProperty(v => v.Description, dnb =>
        {
            dnb.Property(e => e.Value)
                .IsRequired()
                .HasColumnName("description")
                .HasMaxLength(Constants.MAX_HIGH_TEXT_LENGTH);
        });


        builder.ComplexProperty(v => v.Experience, enb =>
        {
            enb.Property(e => e.Experience)
                .IsRequired()
                .HasColumnName("experience");
        });

        builder.ComplexProperty(v => v.PhoneNumber, vnb =>
        {
            vnb.Property(pn => pn.Value)
                .IsRequired()
                .HasColumnName("phone_number");
        });

        builder.HasMany(v => v.Pets)
            .WithOne()
            .HasForeignKey("volunteer_id")
            .IsRequired();

        builder.Property(v => v.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(v => v.DeletionDate);

        //builder.HasQueryFilter(v=>v.IsDeleted)
    }
}