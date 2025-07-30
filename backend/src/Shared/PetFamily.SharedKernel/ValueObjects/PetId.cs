namespace PetFamily.SharedKernel.ValueObjects;

public record PetId : IComparable<PetId>
{
    private PetId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }
    public static PetId NewPetId() => new(Guid.NewGuid());
    public static PetId Empty() => new(Guid.Empty);
    public static PetId Create(Guid id) => new(id);

    public int CompareTo(PetId? other)
    {
        if (ReferenceEquals(this, other)) return 0;
        if (other is null) return 1;
        return Value.CompareTo(other.Value);
    }
}