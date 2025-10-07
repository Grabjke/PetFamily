using PetFamily.Discussions.Application.Discussions.Queries.GetByRelationId;

namespace PetFamily.Discussions.Presentation.Requests;

public record GetByRelationIdRequest(Guid RelationId)
{
    public GetByRelationIdQuery ToQuery()
        => new(RelationId);
}