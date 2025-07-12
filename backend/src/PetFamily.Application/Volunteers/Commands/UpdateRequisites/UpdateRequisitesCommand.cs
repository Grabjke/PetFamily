using PetFamily.Application.Abstractions;
using PetFamily.Application.Dtos;

namespace PetFamily.Application.Volunteers.Commands.UpdateRequisites;

public record UpdateRequisitesCommand(
    Guid VolunteerId,
    IEnumerable<RequisitesDto> Requisites):ICommand;