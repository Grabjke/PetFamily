using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Volunteers.Commands.Delete.Hard;

namespace PetFamily.App.IntegrationTests.VolunteerTests;

public class DeleteVolunteerHandlerTests : VolunteerTestBase
{
    private readonly ICommandHandler<Guid, DeleteVolunteerCommand> _sut;
    
    public DeleteVolunteerHandlerTests(IntegrationTestsWebFactory factory) : base(factory)
    {
        _sut = _scope.ServiceProvider.GetRequiredService<ICommandHandler<Guid, DeleteVolunteerCommand>>();
    }

    [Fact]
    public async Task Success_hard_delete_volunteer()
    {
        // Arrange
        var volunteerId = await DatabaseSeeder.SeedVolunteer(_writeDbContext);
        var command = new DeleteVolunteerCommand(volunteerId);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);
        await _writeDbContext.SaveChangesAsync();

        // Assert
        result.IsSuccess.Should().BeTrue();

        var volunteer = await _writeDbContext.Volunteers
            .FirstOrDefaultAsync(v => v.Id == volunteerId);

        volunteer.Should().BeNull();
    }
    
}