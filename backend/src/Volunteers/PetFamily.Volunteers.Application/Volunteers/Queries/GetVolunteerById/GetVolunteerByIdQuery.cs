using PetFamily.Core.Abstractions;

namespace PetFamily.Volunteers.Application.Volunteers.Queries.GetVolunteerById;

public record GetVolunteerByIdQuery(Guid VolunteerId) : IQuery;