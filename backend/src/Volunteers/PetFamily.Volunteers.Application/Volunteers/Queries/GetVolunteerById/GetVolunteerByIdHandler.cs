using Microsoft.EntityFrameworkCore;
using PetFamily.Core;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Dtos.Query;

namespace PetFamily.Volunteers.Application.Volunteers.Queries.GetVolunteerById;

public class GetVolunteerByIdHandler :
    IQueryHandler<VolunteerDto, GetVolunteerByIdQuery>
{
    private readonly IVolunteersReadDbContext _context;

    public GetVolunteerByIdHandler(IVolunteersReadDbContext context)
    {
        _context = context;
    }
    public async Task<VolunteerDto> Handle(
        GetVolunteerByIdQuery query,
        CancellationToken cancellationToken = default)
    {
        return (await _context.Volunteers
            .FirstOrDefaultAsync(v=>v.Id == query.VolunteerId,  cancellationToken))!;
    }
}