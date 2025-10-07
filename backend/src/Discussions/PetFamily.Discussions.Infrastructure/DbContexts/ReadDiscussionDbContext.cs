using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFamily.Core;
using PetFamily.Core.Dtos.Query;

namespace PetFamily.Discussions.Infrastructure.DbContexts;

public class ReadDiscussionDbContext(string connectionString) : DbContext, IDiscussionReadDbContext
{
    public IQueryable<DiscussionDto> Discussions => Set<DiscussionDto>();
    public IQueryable<MessageDto> Messages => Set<MessageDto>();

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
            type => type.FullName?.Contains("Configurations.Read") ?? false);
    }

    private ILoggerFactory CreateLoggerFactory() =>
        LoggerFactory.Create(builder => { builder.AddConsole(); });
}