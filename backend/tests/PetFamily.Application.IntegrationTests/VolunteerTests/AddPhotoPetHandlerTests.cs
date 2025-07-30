


using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstractions;
using PetFamily.Species.Infrastructure.DbContexts;
using PetFamily.Volunteers.Application.Volunteers.Commands.AddPhotoPet;

namespace PetFamily.App.IntegrationTests.VolunteerTests;

public class AddPhotoPetHandlerTests : VolunteerTestBase
{
    private readonly ICommandHandler<List<string>,AddPhotoPetCommand> _sut;
    
    public AddPhotoPetHandlerTests(IntegrationTestsWebFactory factory) : base(factory)
    {
        _sut = _scope.ServiceProvider.GetRequiredService<ICommandHandler<List<string>, AddPhotoPetCommand>>();
    }

    [Fact]
    public async Task Success_add_photo_to_pet()
    {
        //Arrange
        var volunteerId = await DatabaseSeeder.SeedVolunteer(WriteVolunteerDbContext);
        var (speciesId, breedId) = await DatabaseSeeder.SeedSpeciesAndBreed(_writeSpeciesDbContext);
        var petId = await DatabaseSeeder.SeedPet(WriteVolunteerDbContext,volunteerId, speciesId, breedId);
        var command = _fixture.BuildAddPhotoPetCommand(volunteerId, petId);
            
        //Act
        var result = await _sut.Handle(command, CancellationToken.None);
        //Assert
        var volunteer = await WriteVolunteerDbContext.Volunteers
            .Include(v => v.Pets)
            .FirstOrDefaultAsync();

        var pet = await _readVolunteerDbContext.Pets
            .FirstOrDefaultAsync(p => p.SpeciesId == speciesId && p.BreedId == breedId);
        
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();
        volunteer.Should().NotBeNull();
        volunteer.Pets.Should().HaveCount(1);
        pet.Should().NotBeNull();
        pet.Photos.Should().NotBeNull();
        pet.Photos.Length.Should().Be(2);
        pet.BreedId.Should().Be(breedId);
        pet.SpeciesId.Should().Be(speciesId);
        pet.VolunteerId.Should().Be(volunteerId);
    }
    
    [Fact]
    public async Task Failure_add_photo_to_pet_because_invalid_file_format()
    {
        //Arrange
        var volunteerId = await DatabaseSeeder.SeedVolunteer(WriteVolunteerDbContext);
        var (speciesId, breedId) = await DatabaseSeeder.SeedSpeciesAndBreed(_writeSpeciesDbContext);
        var petId = await DatabaseSeeder.SeedPet(WriteVolunteerDbContext,volunteerId, speciesId, breedId);
        var command = _fixture.BuildAddPhotoPetCommandWithInvalidFormat(volunteerId, petId);
        //Act
        var result = await _sut.Handle(command, CancellationToken.None);
        //Assert
        var volunteer = await WriteVolunteerDbContext.Volunteers
            .Include(v => v.Pets)
            .FirstOrDefaultAsync();

        var pet = await _readVolunteerDbContext.Pets
            .FirstOrDefaultAsync(p => p.SpeciesId == speciesId && p.BreedId == breedId);
        
        result.IsFailure.Should().BeTrue();
        volunteer.Should().NotBeNull();
        volunteer.Pets.Should().HaveCount(1);
        pet.Should().NotBeNull();
        pet.Photos.Should().BeEmpty();
        pet.Photos.Length.Should().Be(0);
        pet.BreedId.Should().Be(breedId);
        pet.SpeciesId.Should().Be(speciesId);
        pet.VolunteerId.Should().Be(volunteerId);
    }
    
}