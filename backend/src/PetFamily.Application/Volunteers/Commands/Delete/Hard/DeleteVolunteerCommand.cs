using PetFamily.Application.Abstractions;

namespace PetFamily.Application.Volunteers.Commands.Delete.Hard;

public record DeleteVolunteerCommand(Guid VolunteerId):ICommand;