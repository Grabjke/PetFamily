
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Accounts.Domain;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;

namespace PetFamily.Accounts.Infrastructure.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");
        
        builder.Property(v => v.SocialNetworks)
            .JsonValueObjectCollectionConversion()
            .IsRequired()
            .HasColumnName("social_networks")
            .HasMaxLength(Constants.MAX_HIGH_TEXT_LENGTH);

        builder.HasMany(u => u.Roles)
            .WithMany()
            .UsingEntity<IdentityUserRole<Guid>>();

        builder.HasOne(u => u.ParticipantAccount)
            .WithOne()
            .HasForeignKey<ParticipantAccount>(p => p.UserId)
            .IsRequired(false);
        
        builder.HasOne(u => u.VolunteerAccount)
            .WithOne()
            .HasForeignKey<VolunteerAccount>(p => p.UserId)
            .IsRequired(false);

        builder.Property(u => u.BannedApplicationUntil)
            .HasColumnName("banned_application_until")
            .SetDefaultDateTimeKind(DateTimeKind.Utc);
    }
}
