using PetFamily.Core.Abstractions;

namespace PetFamily.Discussions.Application.Discussions.Queries.GetByRelationId;

public record GetByRelationIdQuery(Guid RelationId) : IQuery;