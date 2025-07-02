namespace PetFamily.Application.Volunteers.RemovePhotoPet;

public record RemovePetPhotoCommand(Guid VolunteerId,Guid PetId, IEnumerable<string> PhotoNames);