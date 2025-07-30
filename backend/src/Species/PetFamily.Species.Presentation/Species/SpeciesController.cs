using Microsoft.AspNetCore.Mvc;
using PetFamily.Framework;
using PetFamily.Species.Application.Species.Commands.RemoveBreeds;
using PetFamily.Species.Application.Species.Commands.RemoveSpecies;
using PetFamily.Species.Application.Species.Queries.GetBreedsBySpeciesId;
using PetFamily.Species.Application.Species.Queries.GetSpeciesWithPagination;
using PetFamily.Species.Presentation.Species.Requests;

namespace PetFamily.Species.Presentation.Species;

public class SpeciesController : ApplicationController
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
    public async Task<ActionResult<Guid>> RemoveBreed(
        [FromRoute] Guid speciesId,
        [FromRoute] Guid breedId,
        [FromServices] RemoveBreedHandler handler,
        CancellationToken cancellationToken = default)
    {
        var command = new RemoveBreedCommand(speciesId, breedId);

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