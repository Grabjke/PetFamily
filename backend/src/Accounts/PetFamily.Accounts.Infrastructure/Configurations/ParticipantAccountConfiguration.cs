using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Accounts.Domain;
using PetFamily.Core.Extensions;

namespace PetFamily.Accounts.Infrastructure.Configurations;

public class ParticipantAccountConfiguration : IEntityTypeConfiguration<ParticipantAccount>
{
    public void Configure(EntityTypeBuilder<ParticipantAccount> builder)
    {
        builder.ToTable("participant_accounts");

        builder.HasKey(pa => pa.Id);

        builder.Property(pa => pa.UserId)
            .IsRequired();

        builder.Property(pa => pa.FavoritePetsIds)!
            .JsonValueObjectCollectionConversion()
            .IsRequired(false);
    }
}