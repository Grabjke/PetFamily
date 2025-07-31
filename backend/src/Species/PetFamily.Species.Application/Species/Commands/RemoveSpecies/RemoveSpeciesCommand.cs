using PetFamily.Core.Abstractions;

namespace PetFamily.Species.Application.Species.Commands.RemoveSpecies;

public record RemoveSpeciesCommand(Guid SpeciesId) : ICommand;