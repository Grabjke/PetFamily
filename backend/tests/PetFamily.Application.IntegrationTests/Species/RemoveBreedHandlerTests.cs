using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Species.Commands.RemoveBreeds;

namespace PetFamily.App.IntegrationTests.Species;

public class RemoveBreedHandlerTests:SpeciesTestBase
{
    private readonly ICommandHandler<Guid, RemoveBreedCommand> _sut;

    public RemoveBreedHandlerTests(IntegrationTestsWebFactory factory) : base(factory)
    {
        _sut = _scope.ServiceProvider.GetRequiredService<ICommandHandler<Guid, RemoveBreedCommand>>();
    }
    
    [Fact]
    public async Task Should_remove_species()
    {
        //Arrange
        var (speciesId,breedId) = await DatabaseSeeder.SeedSpeciesAndBreed(_writeDbContext);
        var command = new RemoveBreedCommand(speciesId,breedId);
        //Act
        var result = await _sut.Handle(command, CancellationToken.None);
        //Assert
        var species = await _writeDbContext.Species.FirstOrDefaultAsync();
        var breed =  species!.Breeds.FirstOrDefault();
        
        result.IsSuccess.Should().BeTrue();
        species.Should().NotBeNull();
        breed.Should().BeNull();
    }
}