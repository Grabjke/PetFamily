namespace PetFamily.Core.Dtos.Query;

public class VolunteerDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string SurName { get; init; } = string.Empty;

    public string? Patronymic { get; init; } = string.Empty;

    public string Description { get; init; } = string.Empty;
    
    public int Experience { get; init; }
    public RequisitesDto[] Requisites { get; init; } = [];
    public SocialNetworksDto[] SocialNetworks { get; init; } = [];

    public string Email { get; init; } = string.Empty;
    public string PhoneNumber { get; init; } = string.Empty;
}