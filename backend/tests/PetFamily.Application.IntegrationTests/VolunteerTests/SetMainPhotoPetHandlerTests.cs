
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstractions;
using PetFamily.Volunteers.Application.Volunteers.Commands.SetMainPhotoPet;

namespace PetFamily.App.IntegrationTests.VolunteerTests;

public class SetMainPhotoPetHandlerTests : VolunteerTestBase
{
    private readonly ICommandHandler<Guid, SetMainPhotoPetCommand> _sut;

    public SetMainPhotoPetHandlerTests(IntegrationTestsWebFactory factory) : base(factory)
    {
        _sut = _scope.ServiceProvider.GetRequiredService<ICommandHandler<Guid, SetMainPhotoPetCommand>>();
    }

    [Fact]
    public async Task Success_main_photo_pet()
    {
        //Arrange
        var volunteerId = await DatabaseSeeder.SeedVolunteer(WriteVolunteerDbContext);
        var (speciesId, breedId) = await DatabaseSeeder.SeedSpeciesAndBreed(_writeSpeciesDbContext);
        var petId = await DatabaseSeeder.SeedPet(WriteVolunteerDbContext, volunteerId, speciesId, breedId);
        var path = await DatabaseSeeder.SeedPetPhoto(WriteVolunteerDbContext, volunteerId, petId);
        var command = new SetMainPhotoPetCommand(volunteerId, petId, path);
        //Act
        var result = await _sut.Handle(command, CancellationToken.None);
        //Assert
        var volunteer = await WriteVolunteerDbContext.Volunteers
            .Include(v => v.Pets)
            .FirstOrDefaultAsync();
        var pet = volunteer!.Pets.FirstOrDefault();
        var photo = pet!.Photos.FirstOrDefault();
        
        result.IsSuccess.Should().BeTrue();
        pet!.Photos.Should().HaveCount(1);
        photo!.IsMain.Should().BeTrue();
    }
}