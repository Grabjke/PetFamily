using PetFamily.Application.Abstractions;

namespace PetFamily.Application.Volunteers.Commands.Delete.Soft;

public record SoftDeleteVolunteerCommand(Guid VolunteerId) : ICommand;