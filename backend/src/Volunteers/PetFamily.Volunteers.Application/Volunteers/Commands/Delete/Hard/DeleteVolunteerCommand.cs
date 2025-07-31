using PetFamily.Core.Abstractions;

namespace PetFamily.Volunteers.Application.Volunteers.Commands.Delete.Hard;

public record DeleteVolunteerCommand(Guid VolunteerId):ICommand;