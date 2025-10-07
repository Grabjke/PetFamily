using PetFamily.Core.Dtos.Query;

namespace PetFamily.Core;

public interface ISpeciesReadDbContext
{
    public IQueryable<SpeciesDto> Species { get; }
    public IQueryable<BreedDto> Breeds { get; }
}

public interface IVolunteersReadDbContext
{
    public IQueryable<VolunteerDto> Volunteers { get; }
    public IQueryable<PetDto> Pets { get; }
}
public interface IApplicationReadDbContext
{
    public IQueryable<ApplicationDto> Applications { get; }
}

public interface IDiscussionReadDbContext
{
    public IQueryable<DiscussionDto> Discussions { get; }
    public IQueryable<MessageDto> Messages { get; }
    
}