using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Accounts.Domain;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;

namespace PetFamily.Accounts.Infrastructure.Configurations;

public class VolunteerAccountConfiguration : IEntityTypeConfiguration<VolunteerAccount>
{
    public void Configure(EntityTypeBuilder<VolunteerAccount> builder)
    {
        builder.ToTable("volunteer_accounts");

        builder.HasKey(v => v.Id);

        builder.Property(v => v.Expirience)
            .IsRequired();
        
        builder.Property(v => v.Requisites)
            .JsonValueObjectCollectionConversion()
            .IsRequired()
            .HasColumnName("requisites")
            .HasMaxLength(Constants.MAX_HIGH_TEXT_LENGTH);
    }
}