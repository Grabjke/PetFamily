using CSharpFunctionalExtensions;
using PetFamily.Core;
using PetFamily.SharedKernel;

namespace PetFamily.Volunteers.Domain.PetManagement.ValueObjects.Volunteer;

public record VolunteerExperience
{
    //ef
    private VolunteerExperience()
    {
    }
    private VolunteerExperience(int number)
    {
        Experience = number;
    }
    public int Experience { get; }

    public static Result<VolunteerExperience, Error> Create(int number)
    {
        if (number < 0)
            return Errors.General.ValueIsInvalid("Experience");

        return new VolunteerExperience(number);
    }
}