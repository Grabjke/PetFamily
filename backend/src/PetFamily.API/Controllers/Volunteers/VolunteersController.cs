using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Controllers.Volunteers.Requests;
using PetFamily.API.Extensions;
using PetFamily.Application.Volunteers.Create;
using PetFamily.Application.Volunteers.Delete;
using PetFamily.Application.Volunteers.UpdateMainInfo;
using PetFamily.Application.Volunteers.UpdateRequisites;
using PetFamily.Application.Volunteers.UpdateSocialNetworks;

namespace PetFamily.API.Controllers.Volunteers;

[ApiController]
[Route("[controller]")]
public class VolunteersController : ControllerBase
{
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
        var command = new DeleteVolunteerCommand(id);
        
        var result = await handler.Handle(command, cancellationToken);
        
        return result.IsFailure
            ? result.Error.ToResponse()
            : Ok(result.Value);
    }
}