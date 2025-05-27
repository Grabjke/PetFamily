using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Extensions;
using PetFamily.API.Response;
using PetFamily.Application.Volunteers.CreateVolunteer;

namespace PetFamily.API.Controllers;

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
        var result = await handler.Handle(request, cancellationToken);

        if (result.IsSuccess)
            return Ok(result.Value);
    
        return BadRequest(result.Error);

    }
}