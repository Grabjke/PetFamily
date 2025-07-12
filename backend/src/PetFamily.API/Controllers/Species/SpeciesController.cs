using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Controllers.Species.Requests;
using PetFamily.API.Extensions;
using PetFamily.Application.Species.Commands.RemoveBreeds;
using PetFamily.Application.Species.Commands.RemoveSpecies;
using PetFamily.Application.Species.Queries.GetBreedsBySpeciesId;
using PetFamily.Application.Species.Queries.GetSpeciesWithPagination;

namespace PetFamily.API.Controllers.Species;

public class SpeciesController:ApplicationController
{
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<Guid>> DeleteSpecies(
        [FromRoute] Guid id,
        [FromServices] RemoveSpeciesHandler handler,
        CancellationToken cancellationToken = default)
    {
        var command = new RemoveSpeciesCommand(id);
        
        var result = await handler.Handle(command, cancellationToken);
        
        return result.IsFailure
            ? result.Error.ToResponse()
            : Ok(result.Value);
    }
    
    [HttpDelete("{speciesId:guid}/species/{breedId:guid}/breed")]
    public async Task<ActionResult<Guid>> Update(
        [FromRoute] Guid speciesId,
        [FromRoute] Guid breedId,  
        [FromServices] RemoveBreedHandler handler,
        CancellationToken cancellationToken = default)
    {
        var command=new RemoveBreedCommand(speciesId,breedId);
        
        var result = await handler.Handle(command, cancellationToken);
        
        return result.IsFailure
            ? result.Error.ToResponse()
            : Ok(result.Value);
    }
    
    [HttpGet]
    public async Task<ActionResult> GetSpecies(
        [FromServices] GetSpeciesWithPaginationHandler handler,
        [FromQuery] GetSpeciesWithPaginationRequest request,
        CancellationToken cancellationToken)
    {
        var response = await handler.Handle(request.ToQuery(), cancellationToken);

        return Ok(response);
    }
    
    [HttpGet("{id:guid}")]
    public async Task<ActionResult> GetSpecies(
        [FromRoute] Guid id,
        [FromServices] GetBreedsBySpeciesIdWithPaginationHandler handler,
        [FromQuery] GetBreedsBySpeciesIdWithPaginationRequest request,
        CancellationToken cancellationToken)
    {
        var response = await handler.Handle(request.ToQuery(id), cancellationToken);

        return Ok(response);
    }
}