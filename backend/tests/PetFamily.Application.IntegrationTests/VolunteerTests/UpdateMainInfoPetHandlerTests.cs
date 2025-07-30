

using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstractions;
using PetFamily.Volunteers.Application.Volunteers.Commands.UpdateMainInfoPet;

namespace PetFamily.App.IntegrationTests.VolunteerTests;

public class UpdateMainInfoPetHandlerTests : VolunteerTestBase
{
    private readonly ICommandHandler<Guid, UpdateMainInfoPetCommand> _sut;

    public UpdateMainInfoPetHandlerTests(IntegrationTestsWebFactory factory) : base(factory)
    {
        _sut = _scope.ServiceProvider
            .GetRequiredService<ICommandHandler<Guid, UpdateMainInfoPetCommand>>();
    }

    [Fact]
    public async Task Success_update_main_info_pet()
    {
        //Arrange
        var volunteerId = await DatabaseSeeder.SeedVolunteer(WriteVolunteerDbContext);
        var (speciesId, breedId) = await DatabaseSeeder.SeedSpeciesAndBreed(_writeSpeciesDbContext);
        var petId = await DatabaseSeeder.SeedPet(WriteVolunteerDbContext, volunteerId, speciesId, breedId);
        var command = _fixture.BuildUpdateMainInfoPetCommand(volunteerId, petId, speciesId, breedId);
        //Act
        var result = await _sut.Handle(command, CancellationToken.None);
        //Assert

        var pet = await _readVolunteerDbContext.Pets
            .FirstOrDefaultAsync(p => p.SpeciesId == speciesId && p.BreedId == breedId);

        result.IsSuccess.Should().BeTrue();
        pet!.Name.Should().Be(command.Name);
        pet.Street.Should().Be(command.Address.Street);
        pet.Description.Should().Be(command.Description);
        pet.City.Should().Be(command.Address.City);
    }
}