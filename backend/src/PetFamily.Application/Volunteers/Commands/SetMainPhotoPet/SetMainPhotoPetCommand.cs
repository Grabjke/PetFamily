using PetFamily.Application.Abstractions;

namespace PetFamily.Application.Volunteers.Commands.SetMainPhotoPet;

public record SetMainPhotoPetCommand(
    Guid VolunteerId,
    Guid PetId,
    string PhotoPath) : ICommand;