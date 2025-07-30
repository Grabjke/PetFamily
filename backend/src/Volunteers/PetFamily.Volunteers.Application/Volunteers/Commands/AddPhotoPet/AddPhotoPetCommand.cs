using PetFamily.Core.Abstractions;
using PetFamily.Core.Dtos;

namespace PetFamily.Volunteers.Application.Volunteers.Commands.AddPhotoPet;

public record AddPhotoPetCommand(
    Guid VolunteerId,
    Guid PetId,
    IEnumerable<CreateFileDto> Files) : ICommand;