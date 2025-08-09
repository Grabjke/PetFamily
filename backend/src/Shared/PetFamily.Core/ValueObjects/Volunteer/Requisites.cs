using System.Text.Json.Serialization;
using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;

namespace PetFamily.Core.ValueObjects.Volunteer;

public record Requisites
{
    //ef
    private Requisites() { }
    
    [JsonConstructor]
    private Requisites(string title, string description)
    {
        Title = title;
        Description = description;
    }
    public string Title { get; }
    public string Description { get; }
    
    public static Result<Requisites,Error> Create(string title,string description)
    {
        if (string.IsNullOrWhiteSpace(title))
            return Errors.General.ValueIsInvalid("Title");
        
        if (string.IsNullOrWhiteSpace(description))
            return Errors.General.ValueIsInvalid("Description");
        
        return new Requisites(title, description);
    }
    
}