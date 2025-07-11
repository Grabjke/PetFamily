using PetFamily.Application.Abstractions;

namespace PetFamily.Application.Volunteers.Command.Delete;

public record DeleteVolunteerCommand(Guid VolunteerId):ICommand;