using PetFamily.Application.Abstractions;

namespace PetFamily.Application.Volunteers.Queries.GetPetById;

public record GetPetByIdQuery(Guid PetId) : IQuery;