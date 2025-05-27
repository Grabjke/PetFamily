using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.ValueObjects.Volunteer;

public record FullName
{
    private FullName(string name,string surname,string patronymic)
    {
        Name = name;
        Surname = surname;
        Patronymic = patronymic;
    }
    public string Name { get; }
    public string Surname { get; }
    public string? Patronymic { get; }

    public static Result<FullName,Error> Create(string name, string surname, string patronymic)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Errors.General.ValueIsInvalid("Name");
        
        if (string.IsNullOrWhiteSpace(surname))
            return Errors.General.ValueIsInvalid("Surname");
        

        return new FullName(name, surname, patronymic);
    }
    
}