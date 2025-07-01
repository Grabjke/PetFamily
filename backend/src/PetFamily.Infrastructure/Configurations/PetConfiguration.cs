using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Domain.Entities;
using PetFamily.Domain.Shared;
using PetFamily.Domain.ValueObjects.Breed;
using PetFamily.Domain.ValueObjects.Pet;
using PetFamily.Domain.ValueObjects.Species;
using PetFamily.Domain.Extensions;

namespace PetFamily.Infrastructure.Configurations;

public class PetConfiguration:IEntityTypeConfiguration<Pet>
{
    public void Configure(EntityTypeBuilder<Pet> builder)
    {
        builder.ToTable("pets");
        
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasConversion(
                id => id.Value,
                value => PetId.Create(value));

        builder.ComplexProperty(p => p.Name, nb =>
        {
            nb.Property(p => p.Value)
                .IsRequired()
                .HasColumnName("name")
                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);
        });
        
        builder.ComplexProperty(p => p.Description, db =>
        {
            db.Property(p => p.Value)
                .IsRequired()
                .HasColumnName("description")
                .HasMaxLength(Constants.MAX_HIGH_TEXT_LENGTH);
        });
        
        builder.ComplexProperty(p => p.PetSpeciesBreed, sb =>
        {
            sb.Property(p => p.BreedId)
                .IsRequired()
                .HasColumnName("breed_id")
                .HasConversion(
                    id => id.Value,
                    value => BreedId.Create(value));

            sb.Property(p => p.SpeciesId)
                .IsRequired()
                .HasColumnName("species_id")
                .HasConversion(
                    id => id.Value,
                    value => SpeciesId.Create(value));
        });

        builder.ComplexProperty(p => p.Colour, cb =>
        {
            cb.Property(p => p.Value)
                .IsRequired()
                .HasColumnName("colour")
                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);
        });
        
        builder.ComplexProperty(p => p.HealthInformation, hib =>
        {
            hib.Property(p => p.Value)
                .IsRequired()
                .HasColumnName("health_information")
                .HasMaxLength(Constants.MAX_HIGH_TEXT_LENGTH);
        });

        builder.ComplexProperty(p => p.Address, apb =>
        {
            apb.Property(a => a.City)
                .IsRequired()
                .HasColumnName("city")
                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);
            
            apb.Property(a => a.Country)
                .IsRequired()
                .HasColumnName("country")
                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);
            
            apb.Property(a => a.Street)
                .IsRequired()
                .HasColumnName("street")
                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);
            
            apb.Property(a => a.ZipCode)
                .IsRequired(false)
                .HasColumnName("zip_code")
                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);

        });

        builder.ComplexProperty(p => p.Weight, wb =>
        {
            wb.Property(p => p.Weight)
                .IsRequired()
                .HasColumnName("weight");
        });
        
        builder.ComplexProperty(p => p.Height, hb =>
        {
            hb.Property(p => p.Height)
                .IsRequired()
                .HasColumnName("height");
        });
        
        builder.ComplexProperty(p => p.Position, hb =>
        {
            hb.Property(p => p.Value)
                .IsRequired()
                .HasColumnName("serial_number");
        });

        builder.ComplexProperty(p => p.OwnersPhoneNumber, pnb =>
        {
            pnb.Property(pn => pn.PhoneNumber)
                .IsRequired()
                .HasColumnName("phone_number")
                .HasMaxLength(Constants.PHONE_NUMBER_LENGTH);
        });
        
        builder.Property(p => p.Castration)
            .IsRequired()
            .HasColumnName("castration");
        
        
        builder.ComplexProperty(p => p.Birthday, bnb =>
        {
            bnb.Property(bn => bn.DayOfBirth)
                .HasConversion(
                    v => v.ToUniversalTime(),
                    v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
                )
                .IsRequired()
                .HasColumnName("day_of_birth");
        });
        
        builder.Property(p => p.IsVaccinated)
            .IsRequired()
            .HasColumnName("is_vaccinated");

        builder.Property(p => p.HelpStatus)
            .HasConversion<string>()
            .IsRequired()
            .HasColumnName("help_status");

        builder.Property(p => p.Requisites)
            .JsonValueObjectCollectionConversion()
            .IsRequired()
            .HasColumnName("requisites")
            .HasMaxLength(Constants.MAX_HIGH_TEXT_LENGTH);
        
        builder.Property(p => p.Photos)
            .JsonValueObjectCollectionConversion()
            .IsRequired()
            .HasColumnName("photos")
            .HasMaxLength(Constants.MAX_HIGH_TEXT_LENGTH);

        builder.Property(p => p.DateOfCreation)
            .SetDefaultDateTimeKind(DateTimeKind.Utc)
            .IsRequired()
            .HasColumnName("date_of_creation");


        builder.Property(v => v.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(v => v.DeletionDate);

    }
}