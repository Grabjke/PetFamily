using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using PetFamily.Core;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Caching;
using PetFamily.Core.Dtos.Query;

namespace PetFamily.Volunteers.Application.Volunteers.Queries.GetPetById;

public class GetPetByIdHandler : IQueryHandler<PetDto, GetPetByIdQuery>
{
    private readonly DistributedCacheEntryOptions _options = new()
    {
        SlidingExpiration = TimeSpan.FromMinutes(5)
    };

    private readonly IVolunteersReadDbContext _readDbContext;
    private readonly ICacheService _cacheService;

    public GetPetByIdHandler(
        IVolunteersReadDbContext readDbContext,
        ICacheService cacheService)
    {
        _readDbContext = readDbContext;
        _cacheService = cacheService;
    }

    public async Task<PetDto> Handle(
        GetPetByIdQuery query,
        CancellationToken cancellationToken = default)
    {
        var key = "pet_" + query.PetId;

        var petDto = await _cacheService.GetOrSetAsync(
            key,
            _options,
            async () => await GetPetFromDatabase(query.PetId, cancellationToken),
            cancellationToken);

        return petDto ?? new PetDto();
    }

    private async Task<PetDto?> GetPetFromDatabase(Guid petId, CancellationToken cancellationToken)
    {
        var pet = await _readDbContext.Pets
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == petId, cancellationToken);

        if (pet is null)
            return null;

        var sortedPhotos = pet.Photos
            .OrderByDescending(p => p.IsMain)
            .ToArray();

        return new PetDto
        {
            Id = pet.Id,
            PetName = pet.PetName,
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
            PhoneNumber = pet.PhoneNumber,
            Castration = pet.Castration,
            DayOfBirth = pet.DayOfBirth,
            IsVaccinated = pet.IsVaccinated,
            HelpStatus = pet.HelpStatus,
            Requisites = pet.Requisites,
            Photos = sortedPhotos
        };
    }
}