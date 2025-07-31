using PetFamily.Core.Abstractions;

namespace PetFamily.Volunteers.Application.Volunteers.Queries.GetPetById;

public record GetPetByIdQuery(Guid PetId) : IQuery;