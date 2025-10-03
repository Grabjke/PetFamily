using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.SharedKernel;
using PetFamily.VolunteersApplications.Application.VolunteersApplications;
using PetFamily.VolunteersApplications.Domain.ApplicationManagement;
using PetFamily.VolunteersApplications.Infrastructure.DbContexts;

namespace PetFamily.VolunteersApplications.Infrastructure.Repositories;

public class VolunteerApplicationsRepository : IVolunteerApplicationsRepository
{
    private readonly WriteApplicationDbContext _context;

    public VolunteerApplicationsRepository(WriteApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> AddApplication(VolunteerApplication application, CancellationToken cancellationToken)
    {
        await _context.AddAsync(application, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return application.Id;
    }

    public async Task<Result<VolunteerApplication, Error>> GetById(
        Guid applicationId,
        CancellationToken cancellationToken = default)
    {
        var application = await _context.VolunteerApplication
            .FirstOrDefaultAsync(a => a.Id == applicationId, cancellationToken);

        if (application is null)
            return Errors.General.NotFound();

        return application;
    }

    public async Task<Guid> Save(VolunteerApplication application, CancellationToken cancellationToken = default)
    {
        _context.VolunteerApplication.Attach(application);

        await _context.SaveChangesAsync(cancellationToken);
        
        return application.Id;
    }
}