using PetFamily.Domain.Shared;

namespace PetFamily.Domain.ValueObjects.Pet;

public record Address
{
    private Address(string street, string city, string country, string zipcode)
    {
        Street = street;
        City = city;
        Country = country;
        ZipCode = zipcode;
    }
    public string Street { get; }
    public string City { get; }
    public string Country { get; }
    public string? ZipCode { get; }

    public static Result<Address> Create(string street, string city, string country, string zipCode)
    {
        if (string.IsNullOrWhiteSpace(street))
            return "Street cannot be empty";
        
        if (string.IsNullOrWhiteSpace(city))
            return "City cannot be empty";
        
        if (string.IsNullOrWhiteSpace(country))
            return "Country cannot be empty";

        var address = new Address(street, city, country, zipCode);

        return address;

    }

 
}