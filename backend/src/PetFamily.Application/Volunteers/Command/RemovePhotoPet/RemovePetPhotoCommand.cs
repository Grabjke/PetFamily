using PetFamily.Application.Abstractions;

namespace PetFamily.Application.Volunteers.Command.RemovePhotoPet;

public record RemovePetPhotoCommand(
    Guid VolunteerId,Guid PetId,
    IEnumerable<string> PhotoNames):ICommand;