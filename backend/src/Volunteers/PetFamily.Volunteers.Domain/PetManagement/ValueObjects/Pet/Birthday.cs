using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;

namespace PetFamily.Volunteers.Domain.PetManagement.ValueObjects.Pet;

public record Birthday
{
    //ef
    private Birthday()
    {
    }

    private Birthday(DateTime dayOfBirth)
    {
        DayOfBirth = dayOfBirth;
    }
    
    public DateTime DayOfBirth { get; }

    public static Result<Birthday, Error> Create(DateTime dayOfBirth)
    {
        if (dayOfBirth > DateTime.Today)
            return Errors.General.ValueIsInvalid("Day of birth");
        
        var minAnimalDate = new DateTime(2000, 1, 1);
        if (dayOfBirth < minAnimalDate)
            return Errors.General.ValueIsInvalid("Day of birth");
        
        var maxAnimalAge = DateTime.Today.AddYears(-30);
        if (dayOfBirth < maxAnimalAge)
            return Errors.General.ValueIsInvalid("Day of bibrth");

        return new Birthday(dayOfBirth);
    }
}