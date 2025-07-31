

using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstractions;
using PetFamily.Volunteers.Application.Volunteers.Commands.Create;

namespace PetFamily.App.IntegrationTests.VolunteerTests;

public class CreateVolunteerHandlerTests : VolunteerTestBase
{
    private readonly ICommandHandler<Guid,  CreateVolunteerCommand> _sut;
    
    public CreateVolunteerHandlerTests(IntegrationTestsWebFactory factory) : base(factory)
    {
        _sut = _scope.ServiceProvider.GetRequiredService<ICommandHandler<Guid, CreateVolunteerCommand>>();
    }

    [Fact]
    public async Task Success_create_volunteer()
    {
        //Arrange
        var command = _fixture.BuildCreateVolunteerCommand();
        //Act
        var result = await _sut.Handle(command, CancellationToken.None);
        //Assert
        var volunteer = await _readVolunteerDbContext.Volunteers
            .FirstOrDefaultAsync();
        result.IsSuccess.Should().BeTrue();
        volunteer.Should().NotBeNull();
        volunteer.Name.Should().Be(command.FullName.Name);
        volunteer.PhoneNumber.Should().Be(command.PhoneNumber);
        volunteer.SocialNetworks.Should().NotBeEmpty();
        volunteer.Requisites.Should().NotBeEmpty();
    }
    
}