using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Controllers.Volunteers.Requests;
using PetFamily.API.Extensions;
using PetFamily.Application.Volunteers.CreateVolunteer;

namespace PetFamily.API.Controllers.Volunteers;

[ApiController]
[Route("[controller]")]
public class VolunteersController:ControllerBase
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
}