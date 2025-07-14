using CSharpFunctionalExtensions;
using PetFamily.Domain.AggregateRoots;
using PetFamily.Domain.Entities;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers;

public interface IVolunteersRepository
{
    public Task<Guid> Add(Volunteer volunteer, CancellationToken cancellationToken = default);
    public Task<Result<Volunteer,Error>> GetById(Guid volunteerId,CancellationToken cancellationToken = default);
    public Task<Result<Volunteer,Error>> GetByName(string name,CancellationToken cancellationToken = default);
    public  Task<Guid> Save(Volunteer volunteer,CancellationToken cancellationToken = default);
    Task<Guid> Delete(Volunteer volunteer,CancellationToken cancellationToken = default);
    Task<Guid> HardDeletePet(Pet pet, CancellationToken cancellationToken = default);
    Task<Result<Guid, Error>> SetMainPhoto(Pet pet, string photoPath, CancellationToken cancellationToken = default);




}