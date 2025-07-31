
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstractions;
using PetFamily.Species.Infrastructure.DbContexts;
using PetFamily.Volunteers.Application.Volunteers.Commands.AddPet;

namespace PetFamily.App.IntegrationTests.VolunteerTests;

public class AddPetHandlerTests : VolunteerTestBase
{
    private readonly ICommandHandler<Guid,  AddPetCommand> _sut;
    
    public AddPetHandlerTests(IntegrationTestsWebFactory factory) : base(factory)
    {
        _sut = _scope.ServiceProvider.GetRequiredService<ICommandHandler<Guid, AddPetCommand>>();
    }

    [Fact]
    public async Task Add_pet_to_database()
    {
        //Arrange
        var volunteerId = await DatabaseSeeder.SeedVolunteer(WriteVolunteerDbContext);
        var (speciesId, breedId) = await DatabaseSeeder.SeedSpeciesAndBreed(_writeSpeciesDbContext);
        var command = _fixture.BuildAddPetCommand(volunteerId, speciesId, breedId);
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
        pet.Name.Should().Be(command.Name);
        pet.BreedId.Should().Be(breedId);
        pet.SpeciesId.Should().Be(speciesId);
        pet.VolunteerId.Should().Be(volunteerId);
    }
    [Fact]
    public async Task Cannot_add_pet_to_database_because_breed_is_not_found()
    {
        //Arrange
        var volunteerId = await DatabaseSeeder.SeedVolunteer(WriteVolunteerDbContext);
        var (speciesId, breedId) = await DatabaseSeeder.SeedSpeciesAndBreed(_writeSpeciesDbContext);
        var command = _fixture.BuildAddPetCommand(volunteerId, speciesId, Guid.NewGuid());
        //Act
        var result = await _sut.Handle(command, CancellationToken.None);
        //Assert
        var volunteer = await WriteVolunteerDbContext.Volunteers
            .Include(v => v.Pets)
            .FirstOrDefaultAsync();

        var pet = await _readVolunteerDbContext.Pets
            .FirstOrDefaultAsync(p => p.SpeciesId == speciesId && p.BreedId == breedId);

        result.IsFailure.Should().BeTrue();
        volunteer!.Pets.Should().HaveCount(0);
        pet.Should().BeNull();
        volunteer.Should().NotBeNull();
        volunteer!.Pets.Should().BeEmpty();
    }
    
}