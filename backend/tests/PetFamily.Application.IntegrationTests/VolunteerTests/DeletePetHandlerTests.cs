using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Volunteers.Commands.DeletePet.Hard;

namespace PetFamily.App.IntegrationTests.VolunteerTests;

public class DeletePetHandlerTests : VolunteerTestBase
{
    private readonly ICommandHandler<Guid, DeletePetCommand> _sut;
    
    public DeletePetHandlerTests(IntegrationTestsWebFactory factory) : base(factory)
    {
        _sut = _scope.ServiceProvider.GetRequiredService<ICommandHandler<Guid, DeletePetCommand>>();
    }

    [Fact]
    public async Task Success_hard_delete_pet()
    {
        // Arrange
        var volunteerId = await DatabaseSeeder.SeedVolunteer(_writeDbContext);
        var (speciesId, breedId) = await DatabaseSeeder.SeedSpeciesAndBreed(_writeDbContext);
        var petId = await DatabaseSeeder.SeedPet(_writeDbContext, volunteerId, speciesId, breedId);

        var command = new DeletePetCommand(volunteerId, petId);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);
        await _writeDbContext.SaveChangesAsync();

        // Assert
        var volunteer = await _writeDbContext.Volunteers
            .Include(v => v.Pets)
            .FirstOrDefaultAsync(v => v.Id == volunteerId);

        var pet = volunteer!.Pets
            .FirstOrDefault(p => p.Id.Value == petId);
        
        result.IsSuccess.Should().BeTrue();
        pet.Should().BeNull();
        volunteer.Should().NotBeNull();
    }
    
}