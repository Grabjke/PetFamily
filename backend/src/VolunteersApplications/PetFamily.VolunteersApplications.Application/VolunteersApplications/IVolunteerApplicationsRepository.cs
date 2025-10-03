using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;
using PetFamily.VolunteersApplications.Domain.ApplicationManagement;

namespace PetFamily.VolunteersApplications.Application.VolunteersApplications;

public interface IVolunteerApplicationsRepository
{
    public Task<Guid> AddApplication(VolunteerApplication application, CancellationToken cancellationToken);
    
    public Task<Result<VolunteerApplication, Error>> GetById(
        Guid applicationId,
        CancellationToken cancellationToken = default);
    
    public Task<Guid> Save(
        VolunteerApplication application,
        CancellationToken cancellationToken = default);
}