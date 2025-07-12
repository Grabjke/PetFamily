using PetFamily.Application.Abstractions;

namespace PetFamily.Application.Volunteers.Commands.RemovePhotoPet;

public record RemovePetPhotoCommand(
    Guid VolunteerId,Guid PetId,
    IEnumerable<string> PhotoNames):ICommand;