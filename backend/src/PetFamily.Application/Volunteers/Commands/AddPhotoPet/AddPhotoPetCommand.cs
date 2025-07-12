using PetFamily.Application.Abstractions;
using PetFamily.Application.Dtos;

namespace PetFamily.Application.Volunteers.Commands.AddPhotoPet;

public record AddPhotoPetCommand(
    Guid VolunteerId,
    Guid PetId,
    IEnumerable<CreateFileDto> Files) : ICommand;