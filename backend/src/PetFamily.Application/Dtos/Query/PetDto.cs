namespace PetFamily.Application.Dtos.Query;

public class PetDto
{
    public Guid Id { get; init; }
    public string Name { get;  init; } = null!;
    public string Description { get;  init; } = null!;
    public Guid SpeciesId { get;  init; }
    public Guid BreedId { get;  init; }
    public string Colour { get;  init; } = null!;
    public string HealthInformation { get;  init; } = null!;
    public string Country { get;  init; } = string.Empty;
    public string City { get;  init; } = string.Empty;
    public string Street { get;  init; } = string.Empty;
    public string? ZipCode { get;  init; } = string.Empty;
    public double Weight { get;  init; }
    public int Height { get;  init; }
    public string OwnersPhoneNumber { get;  init; } = string.Empty;
    public bool Castration { get;  init; }
    public DateTime Birthday { get;  init; }
    public bool IsVaccinated { get;  init; }
    public string HelpStatus { get;  init; } = string.Empty;
    public RequisitesDto[] Requisites = [];
    public PhotoDto[] Photos = [];
    public DateTime DateOfCreation { get;  init; }
    public int Position { get;  init; }
}