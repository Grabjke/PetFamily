using PetFamily.Core.Abstractions;

namespace PetFamily.Species.Application.Species.Commands.RemoveBreeds;

public record RemoveBreedCommand(Guid SpeciesId,Guid BreedId) : ICommand;