

using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstractions;
using PetFamily.Volunteers.Application.Volunteers.Commands.UpdateRequisites;

namespace PetFamily.App.IntegrationTests.VolunteerTests;

public class UpdateRequisitesVolunteerHandlerTests : VolunteerTestBase
{
    private readonly ICommandHandler<Guid, UpdateRequisitesCommand> _sut;

    public UpdateRequisitesVolunteerHandlerTests(IntegrationTestsWebFactory factory) : base(factory)
    {
        _sut = _scope.ServiceProvider.GetRequiredService<ICommandHandler<Guid, UpdateRequisitesCommand>>();
    }

    [Fact]
    public async Task Success_update_requisites_volunteer()
    {
        //Arrange
        var volunteerId = await DatabaseSeeder.SeedVolunteer(WriteVolunteerDbContext);
        var command = _fixture.BuildUpdateRequisitesCommand(volunteerId);
        //Act
        var result = await _sut.Handle(command, CancellationToken.None);
        //Assert
        var volunteer = await WriteVolunteerDbContext.Volunteers.FirstOrDefaultAsync();
        result.IsSuccess.Should().BeTrue();
        volunteer!.Requisites.Should().HaveCount(1);
        volunteer.Requisites.Should().BeEquivalentTo(command.Requisites);
    }
}