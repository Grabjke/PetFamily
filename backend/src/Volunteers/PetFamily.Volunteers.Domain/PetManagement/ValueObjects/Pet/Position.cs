using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;

namespace PetFamily.Volunteers.Domain.PetManagement.ValueObjects.Pet;

public class Position:ValueObject
{
    private Position(int value)
    {
        Value = value;
    }

    public int Value { get; }

    public Result<Position, Error> Forward()
        => Create(Value + 1);

    public Result<Position, Error> Back()
        => Create(Value - 1);

    public static Result<Position, Error> Create(int value)
    {
        if (value < 0)
            return Errors.General.ValueIsInvalid("position");

        return new Position(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
    
    public static implicit operator int(Position position) 
        => position.Value;
    
}