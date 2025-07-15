using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Application.Dtos.Query;
using PetFamily.Domain.Extensions;


namespace PetFamily.Infrastructure.Configurations.Read;

public class VolunteerDtoConfiguration : IEntityTypeConfiguration<VolunteerDto>
{
    public void Configure(EntityTypeBuilder<VolunteerDto> builder)
    {
        builder.ToTable("volunteers");

        builder.HasKey(p => p.Id);

        builder.Property(v => v.PhoneNumber)
            .HasColumnName("phone_number");
        
        builder.Property(v => v.SurName)
            .HasColumnName("surname");

        builder.Property(v => v.Requisites)
            .HasJsonConversion()
            .HasColumnName("requisites");
        
        builder.Property(v => v.SocialNetworks)
            .HasJsonConversion()
            .HasColumnName("social_networks");
        
        
    }
}