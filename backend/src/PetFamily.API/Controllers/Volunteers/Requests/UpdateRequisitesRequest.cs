﻿using PetFamily.Application.Dtos;
using PetFamily.Application.Volunteers.Commands.UpdateRequisites;

namespace PetFamily.API.Controllers.Volunteers.Requests;

public record UpdateRequisitesRequest (
    IEnumerable<RequisitesDto> Requisites)
{
    public UpdateRequisitesCommand ToCommand(Guid id)
        => new(id, Requisites);
}