namespace PetFamily.Application.Volunteers.CreateVolunteer;

public record CreateVolunteerRequest(
    string Name,
    string Surname,
    string? Patronymic,
    string Email,
    string Description,
    int Experience,
    string PhoneNumber,
    IEnumerable<SocialNetworkDto>? SocialNetworks,
    IEnumerable<RequisitesDto>? Requisites);
    
    public record SocialNetworkDto(string URL,string Name);
    public record RequisitesDto(string Title,string Description);