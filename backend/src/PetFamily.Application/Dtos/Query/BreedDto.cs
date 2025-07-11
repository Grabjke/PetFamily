namespace PetFamily.Application.Dtos.Query;

public class BreedDto
{
    //ef
    private BreedDto()
    {
    }
    public Guid Id { get; init; }

    public string Name { get; init; } = string.Empty;
}