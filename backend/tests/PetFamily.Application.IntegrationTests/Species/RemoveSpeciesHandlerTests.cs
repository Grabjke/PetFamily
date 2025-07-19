using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Species.Commands.RemoveSpecies;

namespace PetFamily.App.IntegrationTests.Species;

public class RemoveSpeciesHandlerTests : SpeciesTestBase
{
    private readonly ICommandHandler<Guid, RemoveSpeciesCommand> _sut;

    public RemoveSpeciesHandlerTests(IntegrationTestsWebFactory factory) : base(factory)
    {
        _sut = _scope.ServiceProvider.GetRequiredService<ICommandHandler<Guid, RemoveSpeciesCommand>>();
    }

    [Fact]
    public async Task Should_remove_species()
    {
        //Arrange
        var (speciesId,_) = await DatabaseSeeder.SeedSpeciesAndBreed(_writeDbContext);
        var command = new RemoveSpeciesCommand(speciesId);
        //Act
        var result = await _sut.Handle(command, CancellationToken.None);
        //Assert
        var species = await _writeDbContext.Species.FirstOrDefaultAsync();
        result.IsSuccess.Should().BeTrue();
        species.Should().BeNull();
    }
}