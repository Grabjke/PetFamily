

using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstractions;
using PetFamily.Volunteers.Application.Volunteers.Commands.DeletePet.Hard;

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
        var volunteerId = await DatabaseSeeder.SeedVolunteer(WriteVolunteerDbContext);
        var (speciesId, breedId) = await DatabaseSeeder.SeedSpeciesAndBreed(_writeSpeciesDbContext);
        var petId = await DatabaseSeeder.SeedPet(WriteVolunteerDbContext, volunteerId, speciesId, breedId);

        var command = new DeletePetCommand(volunteerId, petId);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);
        await WriteVolunteerDbContext.SaveChangesAsync();

        // Assert
        var volunteer = await WriteVolunteerDbContext.Volunteers
            .Include(v => v.Pets)
            .FirstOrDefaultAsync(v => v.Id == volunteerId);

        var pet = volunteer!.Pets
            .FirstOrDefault(p => p.Id.Value == petId);
        
        result.IsSuccess.Should().BeTrue();
        pet.Should().BeNull();
        volunteer.Should().NotBeNull();
    }
    
}