

using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstractions;
using PetFamily.Core.ValueObjects.Pet;
using PetFamily.Volunteers.Application.Volunteers.Commands.ChangeStatusPet;

namespace PetFamily.App.IntegrationTests.VolunteerTests;

public class ChangeStatusPetHandlerTests : VolunteerTestBase
{
    private readonly ICommandHandler<Guid,  ChangeStatusPetCommand> _sut;
    
    public ChangeStatusPetHandlerTests(IntegrationTestsWebFactory factory) : base(factory)
    {
        _sut = _scope.ServiceProvider.GetRequiredService<ICommandHandler<Guid, ChangeStatusPetCommand>>();
    }

    [Fact]
    public async Task Success_change_status_pet()
    {
        //Arrange
        var volunteerId = await DatabaseSeeder.SeedVolunteer(WriteVolunteerDbContext);
        var (speciesId, breedId) = await DatabaseSeeder.SeedSpeciesAndBreed(_writeSpeciesDbContext);
        var petId = await DatabaseSeeder.SeedPet(WriteVolunteerDbContext, volunteerId, speciesId, breedId);
        var command = new ChangeStatusPetCommand(volunteerId, petId, 2);
        //Act
        var result = await _sut.Handle(command, CancellationToken.None);
        //Assert
        var volunteer = await WriteVolunteerDbContext.Volunteers
            .Include(v => v.Pets)
            .FirstOrDefaultAsync();

        var pet = await _readVolunteerDbContext.Pets
            .FirstOrDefaultAsync(p => p.SpeciesId == speciesId && p.BreedId == breedId);
        
        result.IsSuccess.Should().BeTrue();
        pet.Should().NotBeNull();
        volunteer.Should().NotBeNull();
        pet.HelpStatus.Should().Be(HelpStatus.LookingForHome.ToString());
    }
    
    [Fact]
    public async Task Failure_change_status_pet_because_status_is_invalid()
    {
        //Arrange
        var volunteerId = await DatabaseSeeder.SeedVolunteer(WriteVolunteerDbContext);
        var (speciesId, breedId) = await DatabaseSeeder.SeedSpeciesAndBreed(_writeSpeciesDbContext);
        var petId = await DatabaseSeeder.SeedPet(WriteVolunteerDbContext, volunteerId, speciesId, breedId);
        var command = new ChangeStatusPetCommand(volunteerId, petId, 999);
        //Act
        var result = await _sut.Handle(command, CancellationToken.None);
        //Assert
        var volunteer = await WriteVolunteerDbContext.Volunteers
            .Include(v => v.Pets)
            .FirstOrDefaultAsync();

        var pet = await _readVolunteerDbContext.Pets
            .FirstOrDefaultAsync(p => p.SpeciesId == speciesId && p.BreedId == breedId);
        
        result.IsSuccess.Should().BeFalse();
        pet.Should().NotBeNull();
        volunteer.Should().NotBeNull();
    }
    
}