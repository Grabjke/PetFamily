using PetFamily.Volunteers.Contracts.Requests;

namespace PetFamily.Volunteers.Contracts;

public interface IVolunteerContract
{
    Task<bool> HasAnimalsWithBreed(
        HasAnimalsWithBreedRequest request,
        CancellationToken cancellationToken);
    
    Task<bool> HasAnimalsWithSpecies(
        HasAnimalsWithSpeciesRequest request,
        CancellationToken cancellationToken);
}