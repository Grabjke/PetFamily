using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetFamily.Core;
using PetFamily.Framework;
using PetFamily.Volunteers.Application.Volunteers.Commands.AddPet;
using PetFamily.Volunteers.Application.Volunteers.Commands.AddPhotoPet;
using PetFamily.Volunteers.Application.Volunteers.Commands.ChangeStatusPet;
using PetFamily.Volunteers.Application.Volunteers.Commands.Create;
using PetFamily.Volunteers.Application.Volunteers.Commands.Delete.Hard;
using PetFamily.Volunteers.Application.Volunteers.Commands.Delete.Soft;
using PetFamily.Volunteers.Application.Volunteers.Commands.DeletePet.Hard;
using PetFamily.Volunteers.Application.Volunteers.Commands.DeletePet.Soft;
using PetFamily.Volunteers.Application.Volunteers.Commands.MovePetPosition;
using PetFamily.Volunteers.Application.Volunteers.Commands.RemovePhotoPet;
using PetFamily.Volunteers.Application.Volunteers.Commands.SetMainPhotoPet;
using PetFamily.Volunteers.Application.Volunteers.Commands.UpdateMainInfo;
using PetFamily.Volunteers.Application.Volunteers.Commands.UpdateMainInfoPet;
using PetFamily.Volunteers.Application.Volunteers.Commands.UpdateRequisites;
using PetFamily.Volunteers.Application.Volunteers.Commands.UpdateSocialNetworks;
using PetFamily.Volunteers.Application.Volunteers.Queries.GetPetById;
using PetFamily.Volunteers.Application.Volunteers.Queries.GetPetsWithPagination;
using PetFamily.Volunteers.Application.Volunteers.Queries.GetVolunteerById;
using PetFamily.Volunteers.Application.Volunteers.Queries.GetVolunteersWithPagination;
using PetFamily.Volunteers.Presentation.Requests;

namespace PetFamily.Volunteers.Presentation;

public class VolunteersController : ApplicationController
{
    [HttpGet("pets/{petId:guid}")]
    public async Task<ActionResult> GetPetById(
        [FromServices] GetPetByIdHandler handler,
        [FromRoute] Guid petId,
        CancellationToken cancellationToken)
    {
        var query = new GetPetByIdQuery(petId);

        var response = await handler.Handle(query, cancellationToken);

        return Ok(response);
    }
    [HttpGet("pets")]
    public async Task<ActionResult> GetPetsWithPagination(
        [FromServices] GetPetsWithPaginationHandler handler,
        [FromQuery] GetPetsWithPaginationRequest request,
        CancellationToken cancellationToken)
    {
        var response = await handler.Handle(request.ToQuery(), cancellationToken);

        return Ok(response);
    }
    
    [HttpPut("volunteers/{volunteerId:guid}/pets/{petId:guid}/set-main-photo")]
    public async Task<ActionResult<Guid>> SetMainPhoto(
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        [FromBody] SetMainPhotoPetRequest request,
        [FromServices] SetMainPhotoPetHandler handler,
        CancellationToken cancellationToken = default)
    {
        var result = await handler.Handle(request.ToCommand(volunteerId, petId), cancellationToken);

        return result.IsFailure
            ? result.Error.ToResponse()
            : Ok(result.Value);
    }
    [HttpDelete("volunteers/{volunteerId:guid}/pets/{petId:guid}/soft-deleted")]
    public async Task<ActionResult<Guid>> SoftDeletePet(
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        [FromServices] SoftDeletePetHandler handler,
        CancellationToken cancellationToken = default)
    {
        var command = new SoftDeletePetCommand(volunteerId, petId);

        var result = await handler.Handle(command, cancellationToken);

        return result.IsFailure
            ? result.Error.ToResponse()
            : Ok(result.Value);
    }
    [HttpDelete("volunteers/{volunteerId:guid}/pets/{petId:guid}/hard-deleted")]
    public async Task<ActionResult<Guid>> HardDeletePet(
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        [FromServices] DeletePetHandler handler,
        CancellationToken cancellationToken = default)
    {
        var command = new DeletePetCommand(volunteerId, petId);

        var result = await handler.Handle(command, cancellationToken);

        return result.IsFailure
            ? result.Error.ToResponse()
            : Ok(result.Value);
    }

    [HttpPut("volunteers/{volunteerId:guid}/pets/{petId:guid}/change-status")]
    public async Task<ActionResult<Guid>> ChangeStatus(
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        [FromBody] ChangeStatusPetRequest request,
        [FromServices] ChangeStatusPetHandler handler,
        CancellationToken cancellationToken = default)
    {
        var result = await handler.Handle(request.ToCommand(volunteerId, petId), cancellationToken);

        return result.IsFailure
            ? result.Error.ToResponse()
            : Ok(result.Value);
    }

    [HttpPut("volunteers/{volunteerId:guid}/pets/{petId:guid}/update-main-info")]
    public async Task<ActionResult<Guid>> UpdatePet(
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        [FromBody] UpdateMainInfoPetRequest request,
        [FromServices] UpdateMainInfoPetHandler handler,
        CancellationToken cancellationToken = default)
    {
        var result = await handler.Handle(request.ToCommand(volunteerId, petId), cancellationToken);

        return result.IsFailure
            ? result.Error.ToResponse()
            : Ok(result.Value);
    }

    [HttpGet]
    public async Task<ActionResult> Get(
        [FromServices] GetVolunteerWithPaginationHandler handler,
        [FromQuery] GetVolunteerWithPaginationRequest request,
        CancellationToken cancellationToken)
    {
        var response = await handler.Handle(request.ToQuery(), cancellationToken);

        return Ok(response);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult> GetById(
        [FromServices] GetVolunteerByIdHandler handler,
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var query = new GetVolunteerByIdQuery(id);

        var response = await handler.Handle(query, cancellationToken);

        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> Create(
        [FromServices] CreateVolunteerHandler handler,
        [FromBody] CreateVolunteerRequest request,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request.ToCommand(), cancellationToken);

        return result.IsFailure
            ? result.Error.ToResponse()
            : Ok(result.Value);
    }

    [HttpPut("{id:guid}/main-info")]
    public async Task<ActionResult<Guid>> Update(
        [FromRoute] Guid id,
        [FromBody] UpdateMainInfoRequest request,
        [FromServices] UpdateMainInfoHandler handler,
        CancellationToken cancellationToken = default)
    {
        var result = await handler.Handle(request.ToCommand(id), cancellationToken);

        return result.IsFailure
            ? result.Error.ToResponse()
            : Ok(result.Value);
    }

    [HttpPut("{id:guid}/requisite")]
    public async Task<ActionResult<Guid>> Update(
        [FromRoute] Guid id,
        [FromBody] UpdateRequisitesRequest request,
        [FromServices] UpdateRequisitesHandler handler,
        CancellationToken cancellationToken = default)
    {
        var result = await handler.Handle(request.ToCommand(id), cancellationToken);

        return result.IsFailure
            ? result.Error.ToResponse()
            : Ok(result.Value);
    }

    [HttpPut("{id:guid}/social-network")]
    public async Task<ActionResult<Guid>> Update(
        [FromRoute] Guid id,
        [FromBody] UpdateSocialNetworksRequest request,
        [FromServices] UpdateSocialNetworksHandler handler,
        CancellationToken cancellationToken = default)
    {
        var result = await handler.Handle(request.ToCommand(id), cancellationToken);

        return result.IsFailure
            ? result.Error.ToResponse()
            : Ok(result.Value);
    }

    [HttpDelete("{id:guid}/hard")]
    public async Task<ActionResult<Guid>> Delete(
        [FromRoute] Guid id,
        [FromServices] DeleteVolunteerHandler handler,
        CancellationToken cancellationToken = default)
    {
        var command = new DeleteVolunteerCommand(id);

        var result = await handler.Handle(command, cancellationToken);

        return result.IsFailure
            ? result.Error.ToResponse()
            : Ok(result.Value);
    }

    [HttpDelete("{id:guid}/soft")]
    public async Task<ActionResult<Guid>> Delete(
        [FromRoute] Guid id,
        [FromServices] SoftDeleteVolunteerHandler handler,
        CancellationToken cancellationToken = default)
    {
        var command = new SoftDeleteVolunteerCommand(id);

        var result = await handler.Handle(command, cancellationToken);

        return result.IsFailure
            ? result.Error.ToResponse()
            : Ok(result.Value);
    }

    [HttpPost("{id:guid}/pet")]
    public async Task<ActionResult<Guid>> AddPet(
        [FromRoute] Guid id,
        [FromServices] AddPetHandler handler,
        [FromBody] AddPetRequest request,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request.ToCommand(id), cancellationToken);

        return result.IsFailure
            ? result.Error.ToResponse()
            : Ok(result.Value);
    }

    [HttpPut("{volunteerId:guid}/pet/{petId:guid}/position")]
    public async Task<ActionResult<Guid>> Update(
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        [FromBody] MovePetPositionRequest request,
        [FromServices] MovePetPositionHandler handler,
        CancellationToken cancellationToken = default)
    {
        var result = await handler.Handle(request.ToCommand(volunteerId, petId), cancellationToken);

        return result.IsFailure
            ? result.Error.ToResponse()
            : Ok(result.Value);
    }

    [HttpPost("/volunteer/{volunteerId:guid}/pet/{petId:guid}/photos")]
    public async Task<ActionResult> UploadPhotos(
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        IFormFileCollection files,
        [FromServices] AddPhotoPetHandler handler,
        CancellationToken cancellationToken)
    {
        await using var fileProcessor = new FormFileProcessor();

        var filesDtos = fileProcessor.Process(files);

        var command = new AddPhotoPetCommand(volunteerId, petId, filesDtos);

        var result = await handler.Handle(command, cancellationToken);

        return result.IsFailure
            ? result.Error.ToResponse()
            : Ok(result.Value);
    }

    [HttpDelete("{volunteerId:guid}/pet/{petId:guid}/photos")]
    public async Task<IActionResult> RemovePhotos(
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        [FromServices] RemovePetPhotoHandler handler,
        IEnumerable<string> photosNames,
        CancellationToken cancellationToken)
    {
        var command = new RemovePetPhotoCommand(volunteerId, petId, photosNames);

        var result = await handler.Handle(command, cancellationToken);

        return result.IsFailure
            ? result.Error.ToResponse()
            : Ok(result.Value);
    }
}