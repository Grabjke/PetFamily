using PetFamily.Application.Abstractions;
using PetFamily.Application.Dtos;

namespace PetFamily.Application.Volunteers.Command.UpdateRequisites;

public record UpdateRequisitesCommand(
    Guid VolunteerId,
    IEnumerable<RequisitesDto> Requisites):ICommand;