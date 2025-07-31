using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Core.Dtos.Query;
using PetFamily.Core.Extensions;

namespace PetFamily.Volunteers.Infrastructure.Configurations.Read;

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