
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.ValueObjects.Volunteer;

public record Requisites
{
    private Requisites(string title, string description)
    {
        Title = title;
        Description = description;
    }
    public string Title { get; }
    public string Description { get; }
    
    public static Result<Requisites> Create(string title,string description)
    {
        if (string.IsNullOrWhiteSpace(title))
            return "Title cannot be empty";
        
        if (string.IsNullOrWhiteSpace(description))
            return "Description cannot be empty";
        
        var requisites = new Requisites(title, description);

        return requisites;
    }
    
}