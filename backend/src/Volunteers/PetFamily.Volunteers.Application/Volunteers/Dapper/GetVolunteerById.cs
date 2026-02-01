using Dapper;
using PetFamily.Core.Dtos.Query;
using PetFamily.Volunteers.Application.Volunteers.Queries.GetVolunteerById;

namespace PetFamily.Volunteers.Application.Volunteers.Dapper;

public class GetVolunteerById
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public GetVolunteerById(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<VolunteerDto?> Handle(
        GetVolunteerByIdQuery query,
        CancellationToken cancellationToken = default)
    {
        var connection = await _dbConnectionFactory.CreateConnectionAsync(cancellationToken);
        
        VolunteerDto? volunteerDto = null;

        var volunteer = await connection.QueryAsync<VolunteerDto, PetDto, VolunteerDto>(
            """
            SELECT 
                v.id,
                v.name,
                v.surname,
                v.patronymic,
                v.description,
                v.experience,
                v.email,
                v.phone_number,
                p.id         ,
                p.volunteer_id ,
                p.pet_name   ,
                p.description   ,
                p.species_id   ,
                p.breed_id     ,
                p.colour     ,
                p.health_information  ,
                p.country       ,
                p.city        ,
                p.street        ,
                p.zip_code   ,
                p.weight       ,
                p.height       ,
                p.phone_number,
                p.castration   ,
                p.day_of_birth  ,
                p.is_vaccinated,
                p.help_status,
                p.date_of_creation,
                p.serial_number
            FROM volunteers.volunteers v
            JOIN volunteers.pets p ON v.id = p.volunteer_id
            WHERE v.id = @id
            """,
            param: new
            {
                id = query.VolunteerId
            },
            splitOn:"id",
            map: (v,p) =>
            {
                volunteerDto ??= v;
                
                volunteerDto.Pets.Add(p);
                
                return volunteerDto;
            });

        return volunteerDto;
    }
}