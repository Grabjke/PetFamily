


using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstractions;
using PetFamily.Volunteers.Application.Volunteers.Commands.Delete.Soft;

namespace PetFamily.App.IntegrationTests.VolunteerTests;

public class SoftDeleteVolunteerHandlerTests:VolunteerTestBase
{
    private readonly ICommandHandler<Guid, SoftDeleteVolunteerCommand> _sut;

    public SoftDeleteVolunteerHandlerTests(IntegrationTestsWebFactory factory) : base(factory)
    {
        _sut = _scope.ServiceProvider.GetRequiredService<ICommandHandler<Guid, SoftDeleteVolunteerCommand>>();
    }
    
    [Fact]
    public async Task Success_soft_delete_volunteer()
    {
        // Arrange
        var volunteerId = await DatabaseSeeder.SeedVolunteer(WriteVolunteerDbContext);
        var command = new SoftDeleteVolunteerCommand(volunteerId);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);
        await WriteVolunteerDbContext.SaveChangesAsync();

        // Assert
        result.IsSuccess.Should().BeTrue();
        
        var volunteer = await WriteVolunteerDbContext.Volunteers
            .FirstOrDefaultAsync(v => v.Id == volunteerId);
    
        volunteer!.IsDeleted.Should().BeTrue();
    }
    
}