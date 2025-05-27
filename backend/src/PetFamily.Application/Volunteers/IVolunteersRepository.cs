using CSharpFunctionalExtensions;
using PetFamily.Domain.Entities;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers;

public interface IVolunteersRepository
{
    public Task<Guid> Add(Volunteer volunteer, CancellationToken cancellationToken = default);

    public Task<Result<Volunteer,Error>> GetById(Volunteer volunteer,CancellationToken cancellationToken = default);
    
    public Task<Result<Volunteer,Error>> GetByName(string name,CancellationToken cancellationToken = default);
    
    
}