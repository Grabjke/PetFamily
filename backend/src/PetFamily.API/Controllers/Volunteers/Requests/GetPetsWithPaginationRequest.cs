using PetFamily.Application.Volunteers.Queries.GetPetsWithPagination;

namespace PetFamily.API.Controllers.Volunteers.Requests;

public record GetPetsWithPaginationRequest(
    Guid? VolunteerId,
    string? Name,
    string? Description,
    Guid? SpeciesId,
    Guid? BreedId,
    string? Colour,
    string? HealthInformation,
    string? Country,
    string? City,
    string? Street,
    string? ZipCode,
    double? Weight,
    int? Height,
    bool? Castration,
    bool? IsVaccinated,
    int? HelpStatus,
    string? SortBy,
    string? SortDirection,
    int Page,
    int PageSize)
{
    public GetPetsWithPaginationQuery ToQuery()
        => new(
            VolunteerId,
            Name,
            Description,
            SpeciesId,
            BreedId,
            Colour,
            HealthInformation,
            Country,
            City,
            Street,
            ZipCode,
            Weight,
            Height,
            Castration,
            IsVaccinated,
            HelpStatus,
            SortBy,
            SortDirection,
            Page,
            PageSize);
}