using Microsoft.AspNetCore.Mvc;
using PetFamily.Framework;
using PetFamily.VolunteersApplications.Application.VolunteersApplications.Commands.Approve;
using PetFamily.VolunteersApplications.Application.VolunteersApplications.Commands.Create;
using PetFamily.VolunteersApplications.Application.VolunteersApplications.Commands.Reject;
using PetFamily.VolunteersApplications.Application.VolunteersApplications.Commands.Take;
using PetFamily.VolunteersApplications.Application.VolunteersApplications.Commands.Update;
using
    PetFamily.VolunteersApplications.Application.VolunteersApplications.Queries.GetApplicationsByAdminIdWithPagination;
using PetFamily.VolunteersApplications.Application.VolunteersApplications.Queries.GetApplicationsByUserIdWithPagination;
using PetFamily.VolunteersApplications.Application.VolunteersApplications.Queries.
    GetApplicationsNotAcceptedWithPagination;
using PetFamily.VolunteersApplications.Presentation.Requests;

namespace PetFamily.VolunteersApplications.Presentation;

public class VolunteersApplicationsController : ApplicationController
{
    [HttpPost]
    public async Task<ActionResult<Guid>> Create(
        [FromServices] CreateApplicationHandler handler,
        [FromBody] CreateApplicationRequest request,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request.ToCommand(), cancellationToken);

        return result.IsFailure
            ? result.Error.ToResponse()
            : Ok(result.Value);
    }

    [HttpPost("consideration")]
    public async Task<ActionResult<Guid>> Take(
        [FromServices] TakeApplicationHandler handler,
        [FromBody] TakeApplicationRequest request,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request.ToCommand(), cancellationToken);

        return result.IsFailure
            ? result.Error.ToResponse()
            : Ok(result.Value);
    }

    [HttpPost("revision")]
    public async Task<ActionResult<Guid>> Revision(
        [FromServices] RejectApplicationHandler handler,
        [FromBody] RejectApplicationRequest request,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request.ToCommand(), cancellationToken);

        return result.IsFailure
            ? result.Error.ToResponse()
            : Ok();
    }

    [HttpPost("rejected")]
    public async Task<ActionResult<Guid>> Reject(
        [FromServices] RejectApplicationHandler handler,
        [FromBody] RejectApplicationRequest request,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request.ToCommand(), cancellationToken);

        return result.IsFailure
            ? result.Error.ToResponse()
            : Ok();
    }

    [HttpPost("confirmation")]
    public async Task<ActionResult<Guid>> Approve(
        [FromServices] ApproveApplicationHandler handler,
        [FromBody] ApproveApplicationRequest request,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request.ToCommand(), cancellationToken);

        return result.IsFailure
            ? result.Error.ToResponse()
            : Ok();
    }

    [HttpPut]
    public async Task<ActionResult<Guid>> Update(
        [FromServices] UpdateApplicationHandler handler,
        [FromBody] UpdateApplicationRequest request,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request.ToCommand(), cancellationToken);

        return result.IsFailure
            ? result.Error.ToResponse()
            : Ok();
    }

    [HttpGet("NotAccepted")]
    public async Task<ActionResult<Guid>> GetApplicationsNotAcceptedWithPagination(
        [FromServices] GetApplicationsNotAcceptedWithPaginationHandler handler,
        [FromQuery] GetApplicationsNotAcceptedWithPaginationRequest request,
        CancellationToken cancellationToken)
    {
        var response = await handler.Handle(request.ToQuery(), cancellationToken);

        return Ok(response);
    }


    [HttpGet("admin/{adminId:guid}/applications")]
    public async Task<ActionResult<Guid>> GetAdminApplications(
        [FromRoute] Guid adminId,
        [FromServices] GetApplicationsByAdminIdWithPaginationHandler handler,
        [FromQuery] GetApplicationsByAdminIdWithPaginationRequest request,
        CancellationToken cancellationToken)
    {
        var response = await handler.Handle(request.ToQuery(adminId), cancellationToken);

        return Ok(response);
    }
    
    [HttpGet("user/{userId:guid}/applications")]
    public async Task<ActionResult<Guid>> GetUserApplications(
        [FromRoute] Guid userId,
        [FromServices] GetApplicationsByUserIdWithPaginationHandler handler,
        [FromQuery] GetApplicationsByUserIdWithPaginationRequest request,
        CancellationToken cancellationToken)
    {
        var response = await handler.Handle(request.ToQuery(userId), cancellationToken);

        return Ok(response);
    }
}