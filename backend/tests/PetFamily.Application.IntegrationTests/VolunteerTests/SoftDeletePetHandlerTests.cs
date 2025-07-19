using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Volunteers.Commands.DeletePet.Soft;

namespace PetFamily.App.IntegrationTests.VolunteerTests;

public class SoftDeletePetHandlerTests : VolunteerTestBase
{
    private readonly ICommandHandler<Guid, SoftDeletePetCommand> _sut;
    
    public SoftDeletePetHandlerTests(IntegrationTestsWebFactory factory) : base(factory)
    {
        _sut = _scope.ServiceProvider.GetRequiredService<ICommandHandler<Guid, SoftDeletePetCommand>>();
    }
   

    [Fact]
    public async Task Success_soft_delete_pet()
    {
        // Arrange
        var volunteerId = await DatabaseSeeder.SeedVolunteer(_writeDbContext);
        var (speciesId, breedId) = await DatabaseSeeder.SeedSpeciesAndBreed(_writeDbContext);
        var petId = await DatabaseSeeder.SeedPet(_writeDbContext, volunteerId, speciesId, breedId);

        var command = new SoftDeletePetCommand(volunteerId, petId);

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
        pet.Should().NotBeNull();
        pet.IsDeleted.Should().BeTrue();
        volunteer.Should().NotBeNull();
    }
    
}