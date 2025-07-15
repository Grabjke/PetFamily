using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Application.Dtos.Query;
using PetFamily.Domain.Extensions;

namespace PetFamily.Infrastructure.Configurations.Read;

public class PetDtoConfiguration : IEntityTypeConfiguration<PetDto>
{
    public void Configure(EntityTypeBuilder<PetDto> builder)
    {
        builder.ToTable("pets");

        builder.HasKey(p => p.Id);

        builder.HasOne<VolunteerDto>()
            .WithMany()
            .HasForeignKey(p => p.VolunteerId);
        
        builder.Property(p => p.Name)
            .HasColumnName("name");
        
        builder.Property(p => p.Description)
            .HasColumnName("description");
        
        builder.Property(p => p.SpeciesId)
            .HasColumnName("species_id");
        
        builder.Property(p => p.BreedId)
            .HasColumnName("breed_id");
        
        builder.Property(p => p.Colour)
            .HasColumnName("colour");
        
        builder.Property(p => p.HealthInformation)
            .HasColumnName("health_information");
        
        builder.Property(p => p.Country)
            .HasColumnName("country");
        
        builder.Property(p => p.City)
            .HasColumnName("city");
        
        builder.Property(p => p.Street)
            .HasColumnName("street");
        
        builder.Property(p => p.ZipCode)
            .HasColumnName("zip_code");
        
        builder.Property(p => p.Weight)
            .HasColumnName("weight");
        
        builder.Property(p => p.Height)
            .HasColumnName("height");
        
        builder.Property(p => p.OwnersPhoneNumber)
            .HasColumnName("phone_number");
        
        builder.Property(p => p.Birthday)
            .HasColumnName("day_of_birth");
        
        builder.Property(p => p.HelpStatus)
            .HasColumnName("help_status");
        
        builder.Property(p => p.Requisites)
            .HasJsonConversion()
            .HasColumnName("requisites");
        
        builder.Property(p => p.Photos)
            .HasJsonConversion()
            .HasColumnName("photos");
        
        builder.Property(p => p.DateOfCreation)
            .HasColumnName("date_of_creation");
        
        builder.Property(p => p.Position)
            .HasColumnName("serial_number");
    }
}