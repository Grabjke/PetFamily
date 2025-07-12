using PetFamily.Application.Abstractions;

namespace PetFamily.Application.Species.Commands.RemoveSpecies;

public record RemoveSpeciesCommand(Guid SpeciesId) : ICommand;