using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Species;
using PetFamily.Domain.ValueObjects.Species;

namespace PetFamily.Infrastructure.Configurations;

public class SpeciesConfiguration:IEntityTypeConfiguration<PetSpecies>
{
    public void Configure(EntityTypeBuilder<PetSpecies> builder)
    {
        builder.ToTable("species");
        
        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id)
            .HasConversion(
                id => id.Value,
                value => SpeciesId.Create(value));

        builder.Property(s => s.Title)
            .IsRequired()
            .HasColumnName("title")
            .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);
        
        builder.HasMany(s => s.Breeds)
            .WithOne()
            .HasForeignKey("species_id");
    }
}