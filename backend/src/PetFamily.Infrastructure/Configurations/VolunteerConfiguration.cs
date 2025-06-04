using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Domain.Entities;
using PetFamily.Domain.Extensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.ValueObjects.Volunteer;

namespace PetFamily.Infrastructure.Configurations;

public class VolunteerConfiguration:IEntityTypeConfiguration<Volunteer>
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
                .HasColumnName("name")
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
            vnb.Property(pn => pn.PhoneNumber)
                .IsRequired()
                .HasColumnName("phone_number")
                .HasMaxLength(Constants.PHONE_NUMBER_LENGTH);
        });

        builder.Property(v => v.SocialNetworks)
            .JsonValueObjectCollectionConversion()
            .IsRequired()
            .HasColumnName("social_networks")
            .HasMaxLength(Constants.MAX_HIGH_TEXT_LENGTH);

        builder.Property(v => v.Requisites)
            .JsonValueObjectCollectionConversion()
            .IsRequired()
            .HasColumnName("requisites")
            .HasMaxLength(Constants.MAX_HIGH_TEXT_LENGTH);

        builder.HasMany(v => v.Pets)
            .WithOne()
            .HasForeignKey("volunteer_id")
            .IsRequired();
        
        builder.Property(v => v.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(v => v.DeletionDate);

    }
}