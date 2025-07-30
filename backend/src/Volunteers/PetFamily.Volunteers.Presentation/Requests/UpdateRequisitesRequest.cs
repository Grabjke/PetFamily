using PetFamily.Core.Dtos;
using PetFamily.Volunteers.Application.Volunteers.Commands.UpdateRequisites;

namespace PetFamily.Volunteers.Presentation.Requests;

public record UpdateRequisitesRequest (
    IEnumerable<RequisitesDto> Requisites)
{
    public UpdateRequisitesCommand ToCommand(Guid id)
        => new(id, Requisites);
}