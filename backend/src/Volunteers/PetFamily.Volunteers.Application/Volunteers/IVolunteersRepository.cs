using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;
using PetFamily.Volunteers.Domain.PetManagement;

namespace PetFamily.Volunteers.Application.Volunteers;

public interface IVolunteersRepository
{
    public Task<Guid> Add(
        Volunteer volunteer,
        CancellationToken cancellationToken = default);

    public Task<Result<Volunteer, Error>> GetById(
        Guid volunteerId,
        CancellationToken cancellationToken = default);

    public Task<Result<Volunteer, Error>> GetByName(
        string name,
        CancellationToken cancellationToken = default);

    public Task<Guid> Save(
        Volunteer volunteer,
        CancellationToken cancellationToken = default);

    Task<Guid> Delete(
        Volunteer volunteer,
        CancellationToken cancellationToken = default);
}