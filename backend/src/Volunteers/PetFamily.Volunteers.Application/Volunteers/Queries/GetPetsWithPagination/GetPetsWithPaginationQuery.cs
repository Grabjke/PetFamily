using PetFamily.Core.Abstractions;

namespace PetFamily.Volunteers.Application.Volunteers.Queries.GetPetsWithPagination;

public record GetPetsWithPaginationQuery(
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
    int PageSize) : IQuery;