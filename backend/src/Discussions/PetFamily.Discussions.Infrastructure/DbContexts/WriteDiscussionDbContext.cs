using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFamily.Discussions.Domain.DiscussionManagement;

namespace PetFamily.Discussions.Infrastructure.DbContexts;

public class WriteDiscussionDbContext(string connectionString) : DbContext
{
    public DbSet<Discussion> VolunteerApplication => Set<Discussion>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(connectionString);
        optionsBuilder.UseSnakeCaseNamingConvention();
        optionsBuilder.UseLoggerFactory(CreateLoggerFactory());
        optionsBuilder.EnableSensitiveDataLogging();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("discussions");

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(WriteDiscussionDbContext).Assembly,
            type => type.FullName?.Contains("Configurations.Write") ?? false);
    }

    private ILoggerFactory CreateLoggerFactory() =>
        LoggerFactory.Create(builder => { builder.AddConsole(); });
}