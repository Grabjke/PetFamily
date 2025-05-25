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
        
        builder.Property(p => p.Name)
            .IsRequired()
            .HasColumnName("name")
            .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);
        
        builder.Property(p => p.Description)
            .IsRequired()
            .HasColumnName("description")
            .HasMaxLength(Constants.MAX_HIGH_TEXT_LENGTH);
        
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

        builder.Property(p => p.Colour)
            .IsRequired()
            .HasColumnName("colour")
            .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);
        
        builder.Property(p => p.HealthInformation)
            .IsRequired()
            .HasColumnName("health_information")
            .HasMaxLength(Constants.MAX_HIGH_TEXT_LENGTH);

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

        builder.Property(p => p.Weight)
            .IsRequired()
            .HasColumnName("weight");
        
        builder.Property(p => p.Height)
            .IsRequired()
            .HasColumnName("height");

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
        
        builder.Property(p => p.Birthday)
            .SetDefaultDateTimeKind(DateTimeKind.Utc)
            .IsRequired()
            .HasColumnName("birthday");
        
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


        builder.Property(p => p.DateOfCreation)
            .SetDefaultDateTimeKind(DateTimeKind.Utc)
            .IsRequired()
            .HasColumnName("date_of_creation");



    }
}