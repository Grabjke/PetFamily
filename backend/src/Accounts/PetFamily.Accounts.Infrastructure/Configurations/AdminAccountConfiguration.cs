using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Accounts.Domain;
using PetFamily.SharedKernel;

namespace PetFamily.Accounts.Infrastructure.Configurations;

public class AdminAccountConfiguration : IEntityTypeConfiguration<AdminAccount>
{
    public void Configure(EntityTypeBuilder<AdminAccount> builder)
    {
        builder.ToTable("admin_accounts");

        builder.HasOne(a => a.User)
            .WithOne()
            .HasForeignKey<AdminAccount>(a => a.UserId);

        builder.ComplexProperty(a => a.FullName, fn =>
        {
            fn.Property(n => n.Name)
                .IsRequired()
                .HasColumnName("name")
                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);

            fn.Property(s => s.Surname)
                .IsRequired()
                .HasColumnName("surname")
                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);

            fn.Property(p => p.Patronymic)
                .IsRequired(false)
                .HasColumnName("patronymic")
                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);
        });
    }
}