using PetFamily.Application.Abstractions;

namespace PetFamily.Application.Species.Commands.RemoveBreeds;

public record RemoveBreedCommand(Guid SpeciesId,Guid BreedId) : ICommand;