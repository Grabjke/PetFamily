using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.Core;
using PetFamily.SharedKernel;
using PetFamily.Volunteers.Application.Volunteers;
using PetFamily.Volunteers.Infrastructure.DbContexts;

namespace PetFamily.Volunteers.Infrastructure.Repositories;

public class VolunteersRepository : IVolunteersRepository
{
    private readonly WriteVolunteerDbContext _context;

    public VolunteersRepository(WriteVolunteerDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Add(Domain.PetManagement.Volunteer volunteer, CancellationToken cancellationToken = default)
    {
        await _context.Volunteers.AddAsync(volunteer, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return volunteer.Id;
    }

    public async Task<Result<Domain.PetManagement.Volunteer, Error>> GetById(
        Guid volunteerId,
        CancellationToken cancellationToken = default)
    {
        var record = await _context.Volunteers
            .Include(v => v.Pets)
            .FirstOrDefaultAsync(v => v.Id == volunteerId, cancellationToken);

        if (record is null)
            return Errors.General.NotFound(volunteerId);

        return record;
    }

    public async Task<Result<Domain.PetManagement.Volunteer, Error>> GetByName(
        string name, 
        CancellationToken cancellationToken = default)
    {
        var record = await _context.Volunteers
            .Include(v => v.Pets)
            .FirstOrDefaultAsync(v => v.FullName.Name == name, cancellationToken);

        if (record is null)
            return Errors.General.NotFound();

        return record;
    }

    public async Task<Guid> Save(
        Domain.PetManagement.Volunteer volunteer,
        CancellationToken cancellationToken = default)
    {
        _context.Volunteers.Attach(volunteer);

        await _context.SaveChangesAsync(cancellationToken);

        return volunteer.Id.Value;
    }

    public async Task<Guid> Delete(
        Domain.PetManagement.Volunteer volunteer,
        CancellationToken cancellationToken = default)
    {
        _context.Remove(volunteer);

        await _context.SaveChangesAsync(cancellationToken);

        return volunteer.Id;
    }

   
}