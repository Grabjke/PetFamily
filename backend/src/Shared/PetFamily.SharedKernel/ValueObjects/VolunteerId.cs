namespace PetFamily.SharedKernel.ValueObjects;

public record VolunteerId : IComparable<VolunteerId>
{
    private VolunteerId(Guid value)
    {
        Value = value;
    }
    
    public Guid Value { get; }
    public static VolunteerId NewVolunteerId() => new(Guid.NewGuid());
    public static VolunteerId Empty() => new(Guid.Empty);
    public static VolunteerId Create(Guid id) => new(id);

    public static implicit operator Guid(VolunteerId volunteerId)
    {
        ArgumentNullException.ThrowIfNull(volunteerId);
        return volunteerId.Value;
    }

    public int CompareTo(VolunteerId? other)
    {
        if (ReferenceEquals(this, other)) return 0;
        if (other is null) return 1;
        return Value.CompareTo(other.Value);
    }
}