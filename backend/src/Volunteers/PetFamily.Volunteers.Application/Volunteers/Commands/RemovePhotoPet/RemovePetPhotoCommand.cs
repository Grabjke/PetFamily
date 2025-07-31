using PetFamily.Core.Abstractions;

namespace PetFamily.Volunteers.Application.Volunteers.Commands.RemovePhotoPet;

public record RemovePetPhotoCommand(
    Guid VolunteerId,
    Guid PetId,
    IEnumerable<string> PhotoNames) : ICommand;