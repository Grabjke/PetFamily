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

    public async Task<Guid> HardDeletePet(Pet pet, CancellationToken cancellationToken = default)
    {
        _context.Entry(pet).State = EntityState.Deleted;

        await _context.SaveChangesAsync(cancellationToken);

        return pet.Id.Value;
    }

    public async Task<Result<Guid, Error>> SetMainPhoto(Pet pet, string photoPath,
        CancellationToken cancellationToken = default)
    {
        var photoToUpdate = pet.Photos
            .FirstOrDefault(p => p.PathToStorage.Path == photoPath);
        
        if (photoToUpdate is null)
            foreach (var photo in pet.Photos.Where(p => p.IsMain))
            {
                photo.IsMain = false;
            }
        
        if (photoToUpdate is not null)
            photoToUpdate.IsMain = true;

        _context.Entry(pet).Property(x => x.Photos).IsModified = true;
        
        await _context.SaveChangesAsync(cancellationToken);

        return pet.Id.Value;
    }
}