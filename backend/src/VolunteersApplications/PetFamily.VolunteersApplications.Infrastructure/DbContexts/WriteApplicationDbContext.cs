using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFamily.VolunteersApplications.Domain.ApplicationManagement;

namespace PetFamily.VolunteersApplications.Infrastructure.DbContexts;

public class WriteApplicationDbContext(string connectionString) : DbContext
{
    public DbSet<VolunteerApplication> VolunteerApplication => Set<VolunteerApplication>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(connectionString);
        optionsBuilder.UseSnakeCaseNamingConvention();
        optionsBuilder.UseLoggerFactory(CreateLoggerFactory());
        optionsBuilder.EnableSensitiveDataLogging();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("applications");

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(WriteApplicationDbContext).Assembly,
            type => type.FullName?.Contains("Configurations.Write") ?? false);
    }

    private ILoggerFactory CreateLoggerFactory() =>
        LoggerFactory.Create(builder => { builder.AddConsole(); });
}