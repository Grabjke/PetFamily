using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace PetFamily.Core.Extensions;

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
    
    public static PropertyBuilder<DateTime?> SetDefaultDateTimeKind(
        this PropertyBuilder<DateTime?> builder,
        DateTimeKind kind)
    {
        return builder.HasConversion(
            v => v.HasValue ? v.Value.ToUniversalTime() : v,
            v => v.HasValue ? DateTime.SpecifyKind(v.Value, kind) : v);
    }


    public static PropertyBuilder<TValueObject> JsonValueObjectConversion<TValueObject>(
        this PropertyBuilder<TValueObject> builder)
    {
        return builder.HasConversion(
            v => JsonSerializer.Serialize(v, JsonSerializerOptions.Default),
            v => JsonSerializer.Deserialize<TValueObject>(v, JsonSerializerOptions.Default)!);
    }

    public static PropertyBuilder<IReadOnlyList<Guid>> HasUuidArrayConversion(
        this PropertyBuilder<IReadOnlyList<Guid>> propertyBuilder)
    {
        return propertyBuilder
            .HasConversion(
                v => v.ToArray(),
                v => v.ToList(),
                new ValueComparer<IReadOnlyList<Guid>>(
                    (c1, c2) => (c1 == null && c2 == null) || (c1 != null && c2 != null && c1.SequenceEqual(c2)),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    c => c.ToList()
                ))
            .HasColumnType("uuid[]");
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


    public static PropertyBuilder<List<T>> HasJsonbListConversion<T>(
        this PropertyBuilder<List<T>> propertyBuilder)
    {
        var converter = new ValueConverter<List<T>, string>(
            v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
            v => JsonSerializer.Deserialize<List<T>>(v, (JsonSerializerOptions?)null) ?? new List<T>());

        var comparer = new ValueComparer<List<T>>(
            (c1, c2) => c1!.SequenceEqual(c2!),
            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v!.GetHashCode())),
            c => c.ToList());

        propertyBuilder.HasConversion(converter);
        propertyBuilder.Metadata.SetValueConverter(converter);
        propertyBuilder.Metadata.SetValueComparer(comparer);

        return propertyBuilder.HasColumnType("jsonb");
    }


    public static PropertyBuilder<T[]> HasJsonConversion<T>(
        this PropertyBuilder<T[]> builder)
    {
        return builder.HasConversion(
                new ValueConverter<T[], string>(
                    v => JsonSerializer.Serialize(v, JsonSerializerOptions.Default),
                    v => JsonSerializer.Deserialize<T[]>(v, JsonSerializerOptions.Default) ?? Array.Empty<T>()),
                new ValueComparer<T[]>(
                    (c1, c2) => (c1 == null && c2 == null) || (c1 != null && c2 != null && c1.SequenceEqual(c2)),
                    c => c!.Aggregate(0, (a, v) => HashCode.Combine(a, v!.GetHashCode())),
                    c => c!.ToArray()
                )
            )
            .HasColumnType("jsonb");
    }

    public static PropertyBuilder<T[]> HasJsonArrayConversion<T>(
        this PropertyBuilder<T[]> builder,
        JsonSerializerOptions? options = null)
        where T : class
    {
        var serializerOptions = options ?? new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false
        };

        return builder.HasConversion(
                new ValueConverter<T[], string>(
                    v => JsonSerializer.Serialize(v, serializerOptions),
                    v => JsonSerializer.Deserialize<T[]>(v, serializerOptions) ?? Array.Empty<T>(),
                    new ConverterMappingHints())
            )
            .HasColumnType("jsonb");
    }
}