using PetFamily.Application.Dtos;

namespace PetFamily.Application.Volunteers.UpdateRequisites;

public record UpdateRequisitesCommand(Guid VolunteerId,IEnumerable<RequisitesDto> Requisites);