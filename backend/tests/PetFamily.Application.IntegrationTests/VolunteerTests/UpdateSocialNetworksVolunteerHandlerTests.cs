using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Volunteers.Commands.UpdateSocialNetworks;

namespace PetFamily.App.IntegrationTests.VolunteerTests;

public class UpdateSocialNetworksVolunteerHandlerTests:VolunteerTestBase
{
    private readonly ICommandHandler<Guid, UpdateSocialNetworksCommand> _sut;


    public UpdateSocialNetworksVolunteerHandlerTests(IntegrationTestsWebFactory factory) : base(factory)
    {
        _sut = _scope.ServiceProvider.GetRequiredService<ICommandHandler<Guid, UpdateSocialNetworksCommand>>();
    }

    [Fact]
    public async Task Success_updates_social_networks_volunteer()
    {
        var volunteerId = await DatabaseSeeder.SeedVolunteer(_writeDbContext);
        var command = _fixture.BuildUpdateSocialNetworksCommand(volunteerId);
        //Act
        var result = await _sut.Handle(command, CancellationToken.None);
        //Assert
        var volunteer=await _writeDbContext.Volunteers.FirstOrDefaultAsync();
        result.IsSuccess.Should().BeTrue();
        volunteer!.SocialNetworks.Should().HaveCount(2);
        volunteer.SocialNetworks.Select(sn => new { sn.Name, Url = sn.URL })
            .Should()
            .BeEquivalentTo(command.SocialNetworks.Select(dto => new { dto.Name, dto.Url }));
    }
}