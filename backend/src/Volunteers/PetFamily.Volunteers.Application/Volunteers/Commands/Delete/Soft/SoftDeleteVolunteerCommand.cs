using PetFamily.Core.Abstractions;

namespace PetFamily.Volunteers.Application.Volunteers.Commands.Delete.Soft;

public record SoftDeleteVolunteerCommand(Guid VolunteerId) : ICommand;