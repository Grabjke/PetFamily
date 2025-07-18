﻿using PetFamily.Application.Volunteers.Commands.MovePetPosition;

namespace PetFamily.API.Controllers.Volunteers.Requests;

public record MovePetPositionRequest(int Position)
{
    public MovePetPositionCommand ToCommand(Guid volunteerId,Guid petId) => new(
        volunteerId,
        petId,
        Position);
}