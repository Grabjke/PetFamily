using PetFamily.Application.Dtos.Query;

namespace PetFamily.Application.Database;

public interface IReadDbContext
{
    public IQueryable<VolunteerDto> Volunteers { get; }
    public IQueryable<SpeciesDto> Species { get; }
    public IQueryable<BreedDto> Breeds { get; }
    public IQueryable<PetDto> Pets { get; }
}