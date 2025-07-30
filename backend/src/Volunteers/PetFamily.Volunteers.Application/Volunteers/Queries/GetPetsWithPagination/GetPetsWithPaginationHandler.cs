using System.Linq.Expressions;
using PetFamily.Core;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Dtos.Query;
using PetFamily.Core.Extensions;
using PetFamily.Core.Models;

namespace PetFamily.Volunteers.Application.Volunteers.Queries.GetPetsWithPagination;

public class GetPetsWithPaginationHandler : IQueryHandler<PagedList<PetDto>, GetPetsWithPaginationQuery>
{
    private readonly IVolunteersReadDbContext _readDbContext;

    public GetPetsWithPaginationHandler(IVolunteersReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }

    public Task<PagedList<PetDto>> Handle(GetPetsWithPaginationQuery query,
        CancellationToken cancellationToken = default)
    {
        var petsQuery = _readDbContext.Pets.AsQueryable();

        petsQuery = petsQuery.WhereIf(
            query.VolunteerId.HasValue,
            p => p.VolunteerId == query.VolunteerId);

        petsQuery = petsQuery.WhereIf(
            !string.IsNullOrWhiteSpace(query.Name),
            p => p.Name.Contains(query.Name!));

        petsQuery = petsQuery.WhereIf(
            !string.IsNullOrWhiteSpace(query.Description),
            p => p.Description.Contains(query.Description!));

        petsQuery = petsQuery.WhereIf(
            query.SpeciesId.HasValue,
            p => p.SpeciesId == query.SpeciesId);

        petsQuery = petsQuery.WhereIf(
            query.BreedId.HasValue,
            p => p.BreedId == query.BreedId);

        petsQuery = petsQuery.WhereIf(
            !string.IsNullOrWhiteSpace(query.Colour),
            p => p.Colour.Contains(query.Colour!));

        petsQuery = petsQuery.WhereIf(
            !string.IsNullOrWhiteSpace(query.HealthInformation),
            p => p.HealthInformation.Contains(query.HealthInformation!));

        petsQuery = petsQuery.WhereIf(
            !string.IsNullOrWhiteSpace(query.Country),
            p => p.Country.Contains(query.Country!));

        petsQuery = petsQuery.WhereIf(
            !string.IsNullOrWhiteSpace(query.City),
            p => p.City.Contains(query.City!));

        petsQuery = petsQuery.WhereIf(
            !string.IsNullOrWhiteSpace(query.Street),
            p => p.Street.Contains(query.Street!));

        petsQuery = petsQuery.WhereIf(
            !string.IsNullOrWhiteSpace(query.ZipCode),
            p => p.ZipCode!.Contains(query.ZipCode!));

        petsQuery = petsQuery.WhereIf(
            query.Weight.HasValue,
            p => p.Weight == query.Weight);

        petsQuery = petsQuery.WhereIf(
            query.Height.HasValue,
            p => p.Height == query.Height);

        petsQuery = petsQuery.WhereIf(
            query.Castration.HasValue,
            p => p.Castration == query.Castration);

        petsQuery = petsQuery.WhereIf(
            query.IsVaccinated.HasValue,
            p => p.IsVaccinated == query.IsVaccinated);

        petsQuery = petsQuery.WhereIf(
            query.HelpStatus.HasValue,
            p => Equals(p.HelpStatus, query.HelpStatus));


        Expression<Func<PetDto, object>> keySelector = query.SortBy?.ToLower()switch
        {
            "name" => x => x.Name,
            "description" => x => x.Description,
            "colour" => x => x.Colour,
            "country" => x => x.Country,
            "city" => x => x.City,
            "street" => x => x.Street,
            "zip_code" => x => x.ZipCode!,
            "castration" => x => x.Castration,
            "is_vaccinated" => x => x.IsVaccinated,
            "help_status" => x => x.HelpStatus,
            _ => x => x.Id
        };

        petsQuery = query.SortDirection?.ToLower() == "desc"
            ? petsQuery.OrderByDescending(keySelector)
            : petsQuery.OrderBy(keySelector);

        return petsQuery.ToPagedList(query.Page, query.PageSize, cancellationToken: cancellationToken);
    }
}