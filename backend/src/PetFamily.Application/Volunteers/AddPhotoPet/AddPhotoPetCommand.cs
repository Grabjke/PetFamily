using PetFamily.Application.Dtos;

namespace PetFamily.Application.Volunteers.AddPhotoPet;

public record AddPhotoPetCommand(
    Guid VolunteerId,
    Guid PetId,
    IEnumerable<CreateFileDto>  Files);