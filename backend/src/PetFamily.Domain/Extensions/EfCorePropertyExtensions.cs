using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace PetFamily.Domain.Extensions;

public static class EfCorePropertyExtensions
{
    public static PropertyBuilder<DateTime> SetDefaultDateTimeKind(
        this PropertyBuilder<DateTime> builder,
        DateTimeKind kind)
    {
        return builder.HasConversion(
            v => v.ToUniversalTime(),
            v => DateTime.SpecifyKind(v, kind));
    }

    
    public static PropertyBuilder<TValueObject> JsonValueObjectConversion<TValueObject>(
        this PropertyBuilder<TValueObject> builder)
    {
        return builder.HasConversion(
            v => JsonSerializer.Serialize(v, JsonSerializerOptions.Default),
            v => JsonSerializer.Deserialize<TValueObject>(v, JsonSerializerOptions.Default)!);
    }
    
    
    public static PropertyBuilder<IReadOnlyList<TValueObject>> JsonValueObjectCollectionConversion<TValueObject>(
        this PropertyBuilder<IReadOnlyList<TValueObject>> builder)
    {
        return builder.HasConversion<string>(
            v => JsonSerializer.Serialize(v, JsonSerializerOptions.Default),
            v => JsonSerializer.Deserialize<IReadOnlyList<TValueObject>>(v, JsonSerializerOptions.Default)!,
            new ValueComparer<IReadOnlyList<TValueObject>>(
                (c1, c2) => c1!.SequenceEqual(c2!),
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v!.GetHashCode())),
                c => c.ToList()
            )
        )
        .HasColumnType("jsonb");
    }
}