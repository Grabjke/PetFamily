namespace PetFamily.Application.Dtos.Query;

public class SpeciesDto
{
    //ef
    private SpeciesDto()
    {
    }
    public Guid Id { get; init; }

    public string Title { get; init; } = string.Empty;

    public BreedDto[]? Breeds = [];
}