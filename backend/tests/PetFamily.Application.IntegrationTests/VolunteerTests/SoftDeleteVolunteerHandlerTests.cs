using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Volunteers.Commands.Delete.Soft;

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
        var volunteerId = await DatabaseSeeder.SeedVolunteer(_writeDbContext);
        var command = new SoftDeleteVolunteerCommand(volunteerId);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);
        await _writeDbContext.SaveChangesAsync();

        // Assert
        result.IsSuccess.Should().BeTrue();
        
        var volunteer = await _writeDbContext.Volunteers
            .FirstOrDefaultAsync(v => v.Id == volunteerId);
    
        volunteer!.IsDeleted.Should().BeTrue();
    }
    
}