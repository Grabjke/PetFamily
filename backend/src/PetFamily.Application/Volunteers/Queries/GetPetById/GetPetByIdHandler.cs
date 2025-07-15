using Microsoft.EntityFrameworkCore;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.Dtos.Query;

namespace PetFamily.Application.Volunteers.Queries.GetPetById;

public class GetPetByIdHandler : IQueryHandler<PetDto, GetPetByIdQuery>
{
    private readonly IReadDbContext _readDbContext;

    public GetPetByIdHandler(IReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }
    
    public async Task<PetDto> Handle(
        GetPetByIdQuery query,
        CancellationToken cancellationToken = default)
    {
        var pet = await _readDbContext.Pets
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == query.PetId, cancellationToken);
        
        var sortedPhotos = pet!.Photos
            .OrderByDescending(p => p.IsMain)
            .ToArray();
        
        return new PetDto
        {
            Id = pet.Id,
            Name = pet.Name,
            VolunteerId = pet.VolunteerId,
            Description = pet.Description,
            SpeciesId = pet.SpeciesId,
            BreedId = pet.BreedId,
            Colour = pet.Colour,
            HealthInformation = pet.HealthInformation,
            Country = pet.Country,
            City = pet.City,
            Street = pet.Street,
            ZipCode = pet.ZipCode,
            Weight = pet.Weight,
            Height = pet.Height,
            OwnersPhoneNumber = pet.OwnersPhoneNumber,
            Castration = pet.Castration,
            Birthday = pet.Birthday,
            IsVaccinated = pet.IsVaccinated,
            HelpStatus = pet.HelpStatus,
            Requisites = pet.Requisites,
            Photos = sortedPhotos
        };
    }
}