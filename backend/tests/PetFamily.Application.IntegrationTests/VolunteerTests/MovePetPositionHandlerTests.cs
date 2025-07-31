
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstractions;
using PetFamily.Volunteers.Application.Volunteers.Commands.MovePetPosition;

namespace PetFamily.App.IntegrationTests.VolunteerTests;

public class MovePetPositionHandlerTests : VolunteerTestBase
{
    private readonly ICommandHandler<Guid, MovePetPositionCommand> _sut;

    public MovePetPositionHandlerTests(IntegrationTestsWebFactory factory) : base(factory)
    {
        _sut = _scope.ServiceProvider.GetRequiredService<ICommandHandler<Guid, MovePetPositionCommand>>();
    }

    [Fact]
    public async Task Success_move_position_pet()
    {
        //Arrange
        var volunteerId = await DatabaseSeeder.SeedVolunteer(WriteVolunteerDbContext);
        var (speciesId, breedId) = await DatabaseSeeder.SeedSpeciesAndBreed(_writeSpeciesDbContext);
        var petIds = await DatabaseSeeder.SeedManyPets(WriteVolunteerDbContext, volunteerId, speciesId, breedId);
        var command = new MovePetPositionCommand(volunteerId, petIds.First(), 3);
        //Act
        var result = await _sut.Handle(command, CancellationToken.None);
        //Assert
        var volunteer = await WriteVolunteerDbContext.Volunteers
            .Include(v => v.Pets)
            .FirstOrDefaultAsync();

        var pet = await _readVolunteerDbContext.Pets
            .FirstOrDefaultAsync(p => p.Id == petIds.First());

        result.IsSuccess.Should().BeTrue();
        pet!.Position.Should().Be(3);
    }
}