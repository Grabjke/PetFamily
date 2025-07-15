using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.Application.Volunteers;
using PetFamily.Domain.AggregateRoots;
using PetFamily.Domain.Entities;
using PetFamily.Domain.Shared;
using PetFamily.Infrastructure.DbContexts;

namespace PetFamily.Infrastructure.Repositories;

public class VolunteersRepository : IVolunteersRepository
{
    private readonly WriteDbContext _context;

    public VolunteersRepository(WriteDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Add(Volunteer volunteer, CancellationToken cancellationToken = default)
    {
        await _context.Volunteers.AddAsync(volunteer, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return volunteer.Id;
    }

    public async Task<Result<Volunteer, Error>> GetById(
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

    public async Task<Result<Volunteer, Error>> GetByName(string name, CancellationToken cancellationToken = default)
    {
        var record = await _context.Volunteers
            .Include(v => v.Pets)
            .FirstOrDefaultAsync(v => v.FullName.Name == name, cancellationToken);

        if (record is null)
            return Errors.General.NotFound();

        return record;
    }

    public async Task<Guid> Save(Volunteer volunteer, CancellationToken cancellationToken = default)
    {
        _context.Volunteers.Attach(volunteer);

        await _context.SaveChangesAsync(cancellationToken);

        return volunteer.Id.Value;
    }

    public async Task<Guid> Delete(Volunteer volunteer, CancellationToken cancellationToken = default)
    {
        _context.Remove(volunteer);

        await _context.SaveChangesAsync(cancellationToken);

        return volunteer.Id;
    }

   
}