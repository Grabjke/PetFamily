using Microsoft.EntityFrameworkCore;
using PetFamily.Core;
using PetFamily.Volunteers.Contracts;
using PetFamily.Volunteers.Contracts.Requests;

namespace PetFamily.Volunteers.Presentation;

public class VolunteerContract : IVolunteerContract
{
    private readonly IVolunteersReadDbContext _readDbContext;

    public VolunteerContract(IVolunteersReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }

    public async Task<bool> HasAnimalsWithBreed(
        HasAnimalsWithBreedRequest request,
        CancellationToken cancellationToken)
    {
        return await _readDbContext.Pets
            .AnyAsync(p => p.BreedId == request.BreedId, cancellationToken);
    }

    public async Task<bool> HasAnimalsWithSpecies(
        HasAnimalsWithSpeciesRequest request,
        CancellationToken cancellationToken)
    {
        return await _readDbContext.Pets
            .AnyAsync(p => p.SpeciesId == request.SpeciesId, cancellationToken);
    }
}