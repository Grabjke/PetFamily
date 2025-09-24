using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;

namespace PetFamily.Core.ValueObjects.Volunteer;

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

    public static Result<FullName,Error> Create(string name, string surname, string? patronymic)
    {
        if (string.IsNullOrWhiteSpace(name)||name.Length > Constants.MAX_LOW_TEXT_LENGTH)
            return Errors.General.ValueIsInvalid("PetName");
        
        if (string.IsNullOrWhiteSpace(surname)|| surname.Length > Constants.MAX_LOW_TEXT_LENGTH)
            return Errors.General.ValueIsInvalid("Surname");
        
        if(patronymic != null && patronymic.Length > Constants.MAX_LOW_TEXT_LENGTH)
            return Errors.General.ValueIsInvalid("Patronymic");
        
        return new FullName(name, surname, patronymic);
    }
    public static Result<FullName, Error> Create(string name, string surname)
    {
        return Create(name, surname, null);
    }
    
}