using PetFamily.Core.Abstractions;
using PetFamily.Core.Dtos;

namespace PetFamily.Volunteers.Application.Volunteers.Commands.UpdateRequisites;

public record UpdateRequisitesCommand(
    Guid VolunteerId,
    IEnumerable<RequisitesDto> Requisites) : ICommand;