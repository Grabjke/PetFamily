using System.Text.Json.Serialization;
using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;

namespace PetFamily.Core.ValueObjects.Volunteer;

public record SocialNetwork
{
    //ef
    public SocialNetwork()
    {
    }
    [JsonConstructor]
    private SocialNetwork(string url,string name)
    {
        URL = url;
        Name = name;
    }
    public string URL { get; }
    public string Name { get; }

    public static Result<SocialNetwork,Error> Create(string url, string name)
    {
        if (string.IsNullOrWhiteSpace(url))
            return Errors.General.ValueIsInvalid("URL");
        
        if (string.IsNullOrWhiteSpace(name))
            return Errors.General.ValueIsInvalid("Name");
        

        return new SocialNetwork(url, name);
    }
}