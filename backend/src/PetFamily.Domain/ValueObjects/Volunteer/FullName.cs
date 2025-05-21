using PetFamily.Domain.Shared;

namespace PetFamily.Domain.ValueObjects.Volunteer;

public record FullName
{
    public string Name { get; }
    public string Surname { get; }
    public string? Patronymic { get; }

    private FullName(string name,string surname,string patronymic)
    {
        Name = name;
        Surname = surname;
        Patronymic = patronymic;
    }

    public static Result<FullName> Create(string name, string surname, string patronymic)
    {
        if (string.IsNullOrWhiteSpace(name))
            return "Name can not be empty";
        
        if (string.IsNullOrWhiteSpace(surname))
            return Result<FullName>.Failure("Surname can not be empty");
        

        var fullName = new FullName(name, surname, patronymic);

        return fullName;
    }
    
}