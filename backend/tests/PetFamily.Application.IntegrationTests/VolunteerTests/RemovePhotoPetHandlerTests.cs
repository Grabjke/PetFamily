using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Volunteers.Commands.RemovePhotoPet;

namespace PetFamily.App.IntegrationTests.VolunteerTests;

public class RemovePhotoPetHandlerTests : VolunteerTestBase
{
    private readonly ICommandHandler<List<string>, RemovePetPhotoCommand> _sut;

    public RemovePhotoPetHandlerTests(IntegrationTestsWebFactory factory) : base(factory)
    {
        _sut = _scope.ServiceProvider.GetRequiredService<ICommandHandler<List<string>, RemovePetPhotoCommand>>();
    }

    [Fact]
    public async Task Success_remove_photo_pet_()
    {
        //Arrange
        var volunteerId = await DatabaseSeeder.SeedVolunteer(_writeDbContext);
        var (speciesId, breedId) = await DatabaseSeeder.SeedSpeciesAndBreed(_writeDbContext);
        var petId = await DatabaseSeeder.SeedPet(_writeDbContext, volunteerId, speciesId, breedId);
        var path = await DatabaseSeeder.SeedPetPhoto(_writeDbContext, volunteerId, petId);
        var command = new RemovePetPhotoCommand(volunteerId, petId, [path]);
        //Act
        var result = await _sut.Handle(command,CancellationToken.None);
        //Assert
        var volunteer = await _writeDbContext.Volunteers
            .Include(v => v.Pets)
            .FirstOrDefaultAsync();

        var pet = volunteer!.Pets.FirstOrDefault();
        
        result.IsSuccess.Should().BeTrue();
        pet!.Photos.Should().BeEmpty(); 
    }
}