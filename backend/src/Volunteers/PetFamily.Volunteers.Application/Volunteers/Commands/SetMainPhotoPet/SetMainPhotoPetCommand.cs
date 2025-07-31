using PetFamily.Core.Abstractions;

namespace PetFamily.Volunteers.Application.Volunteers.Commands.SetMainPhotoPet;

public record SetMainPhotoPetCommand(
    Guid VolunteerId,
    Guid PetId,
    string PhotoPath) : ICommand;