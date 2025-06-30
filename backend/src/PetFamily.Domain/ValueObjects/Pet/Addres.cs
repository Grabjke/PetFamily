using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.ValueObjects.Pet;

public record Address
{
    private Address()
    {
    }

    
    private Address(string street, string city, string country, string? zipcode)
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

    public static Result<Address,Error> Create(string street, string city, string country, string? zipCode=null)
    {
        if (string.IsNullOrWhiteSpace(street))
            return Errors.General.ValueIsInvalid("Street");
        
        if (string.IsNullOrWhiteSpace(city))
            return Errors.General.ValueIsInvalid("City");
        
        if (string.IsNullOrWhiteSpace(country))
            return Errors.General.ValueIsInvalid("Country");
        
        return new Address(street, city, country, zipCode);
    }

 
}