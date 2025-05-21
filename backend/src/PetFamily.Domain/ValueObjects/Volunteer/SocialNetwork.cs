
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.ValueObjects.Volunteer;

public record SocialNetwork
{
    public string URL { get; }
    public string Name { get; }

    private SocialNetwork(string url,string name)
    {
        URL = url;
        Name = name;
    }

    public static Result<SocialNetwork> Create(string url, string name)
    {
        if (string.IsNullOrWhiteSpace(url))
            return "URL is invalid";
        
        if (string.IsNullOrWhiteSpace(name))
            return "Name is invalid";

        var socialNetwork = new SocialNetwork(url, name);

        return socialNetwork;
    }
}