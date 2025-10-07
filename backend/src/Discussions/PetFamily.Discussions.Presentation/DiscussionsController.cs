using Microsoft.AspNetCore.Mvc;
using PetFamily.Discussions.Application.Discussions.Command.Close;
using PetFamily.Discussions.Application.Discussions.Command.Create;
using PetFamily.Discussions.Application.Discussions.Command.DeleteMessage;
using PetFamily.Discussions.Application.Discussions.Command.EditMessage;
using PetFamily.Discussions.Application.Discussions.Command.SendMessage;
using PetFamily.Discussions.Application.Discussions.Queries.GetByRelationId;
using PetFamily.Discussions.Presentation.Requests;
using PetFamily.Framework;

namespace PetFamily.Discussions.Presentation;

public class DiscussionsController : ApplicationController
{
    [HttpPost]
    public async Task<ActionResult<Guid>> Create(
        [FromServices] CreateDiscussionHandler handler,
        [FromBody] CreateDiscussionRequest request,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request.ToCommand(), cancellationToken);

        return result.IsFailure
            ? result.Error.ToResponse()
            : Ok(result.Value);
    }

    [HttpPut]
    public async Task<ActionResult<Guid>> Close(
        [FromServices] CloseDiscussionHandler handler,
        [FromBody] CloseDiscussionRequest request,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request.ToCommand(), cancellationToken);

        return result.IsFailure
            ? result.Error.ToResponse()
            : Ok(result.Value);
    }

    [HttpPost("message")]
    public async Task<ActionResult<Guid>> AddMessage(
        [FromServices] SendMessageHandler handler,
        [FromBody] SendMessageRequest request,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request.ToCommand(), cancellationToken);

        return result.IsFailure
            ? result.Error.ToResponse()
            : Ok(result.Value);
    }

    [HttpPut("message")]
    public async Task<ActionResult<Guid>> EditMessage(
        [FromServices] EditMessageHandler handler,
        [FromBody] EditMessageRequest request,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request.ToCommand(), cancellationToken);

        return result.IsFailure
            ? result.Error.ToResponse()
            : Ok();
    }

    [HttpDelete("message")]
    public async Task<ActionResult<Guid>> DeleteMessage(
        [FromServices] DeleteMessageHandler handler,
        [FromBody] DeleteMessageRequest request,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request.ToCommand(), cancellationToken);

        return result.IsFailure
            ? result.Error.ToResponse()
            : Ok();
    }

    [HttpGet("discussion")]
    public async Task<ActionResult<Guid>> GetDiscussionByRelationId(
        [FromServices] GetByRelationIdHandler handler,
        [FromQuery] GetByRelationIdRequest request,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request.ToQuery(), cancellationToken);

        return Ok(result);
    }
}