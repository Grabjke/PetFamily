using Microsoft.AspNetCore.Mvc;
using PetFamily.Discussions.Application.Discussions.Command.Create;
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
}