

using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstractions;
using PetFamily.Volunteers.Application.Volunteers.Commands.UpdateMainInfo;

namespace PetFamily.App.IntegrationTests.VolunteerTests;

public class UpdateMainInfoVolunteerHandlerTests:VolunteerTestBase
{
    private readonly ICommandHandler<Guid, UpdateMainInfoCommand> _sut;

    public UpdateMainInfoVolunteerHandlerTests(IntegrationTestsWebFactory factory) : base(factory)
    {
        _sut = _scope.ServiceProvider.GetRequiredService<ICommandHandler<Guid, UpdateMainInfoCommand>>();
    }

    [Fact]
    public async Task Success_updates_main_info_volunteer()
    {
        //Arrange
        var volunteerId= await DatabaseSeeder.SeedVolunteer(WriteVolunteerDbContext);
        var command = _fixture.BuildUpdateMainInfoCommand(volunteerId);
        //Act
        var result = await _sut.Handle(command,CancellationToken.None);
        //Assert
        var volunteer = await WriteVolunteerDbContext.Volunteers.FirstOrDefaultAsync();
        result.IsSuccess.Should().BeTrue();
        volunteerId.Should().Be(volunteerId);
        volunteer!.FullName.Name.Should().Be(command.FullName.Name);
        volunteer.FullName.Surname.Should().Be(command.FullName.Surname);
        volunteer.FullName.Patronymic.Should().Be(command.FullName.Patronymic); 
        volunteer.Email.Value.Should().Be(command.Email);
        volunteer.PhoneNumber.Value.Should().Be(command.PhoneNumber);
        
    }
}