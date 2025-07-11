using Microsoft.EntityFrameworkCore;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.Dtos.Query;

namespace PetFamily.Application.Volunteers.Queries.GetVolunteerById;

public class GetVolunteerByIdHandler :
    IQueryHandler<VolunteerDto, GetVolunteerByIdQuery>
{
    private readonly IReadDbContext _context;

    public GetVolunteerByIdHandler(IReadDbContext context)
    {
        _context = context;
    }
    public async Task<VolunteerDto> Handle(
        GetVolunteerByIdQuery command,
        CancellationToken cancellationToken = default)
    {
        return (await _context.Volunteers
            .FirstOrDefaultAsync(v=>v.Id == command.VolunteerId,  cancellationToken))!;
    }
}