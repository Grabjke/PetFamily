using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.Application.Volunteers;
using PetFamily.Domain.Entities;
using PetFamily.Domain.Shared;

namespace PetFamily.Infrastructure.Repositories;

public class VolunteersRepository:IVolunteersRepository
{
    private readonly ApplicationDbContext _context;

    public VolunteersRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Add(Volunteer volunteer, CancellationToken cancellationToken = default)
    {
        await _context.Volunteers.AddAsync(volunteer, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return volunteer.Id;
    }

    public async Task<Result<Volunteer,Error>> GetById(Volunteer volunteer)
    {
        var record = await  _context.Volunteers
            .Include(v => v.Pets)
            .FirstOrDefaultAsync(v=>v.Id==volunteer.Id);

        if (record is null)
            return Errors.General.NotFound(volunteer.Id);

        return record;
    }

    public async Task<Result<Volunteer,Error>> GetByName(string name)
    {
        var record = await  _context.Volunteers
            .Include(v => v.Pets)
            .FirstOrDefaultAsync(v=>v.FullName.Name==name);
        
        if (record is null)
            return Errors.General.NotFound();

        return record;
        
    }
}