using System.Text.Json;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace VitalityBuilder.Api.Infrastructure;

/// <summary>
/// Generic JSON value converter for Entity Framework Core
/// </summary>
public class JsonValueConverter<T> : ValueConverter<T, string> where T : class
{
    public JsonValueConverter() : base(
        // Serialize to JSON string
        v => JsonSerializer.Serialize(v, JsonSerializerOptions.Default),
        // Deserialize from JSON string with null handling
        v => v == null ? null! : JsonSerializer.Deserialize<T>(v, JsonSerializerOptions.Default)!
    )
    {
    }
}

/// <summary>
/// Generic JSON comparer for Entity Framework Core
/// </summary>
public class JsonValueComparer<T> : ValueComparer<T> where T : class
{
    private static bool EqualsJson(T? left, T? right)
    {
        if (left == null && right == null)
            return true;
        if (left == null || right == null)
            return false;
        return JsonSerializer.Serialize(left) == JsonSerializer.Serialize(right);
    }

    private static int GetHashCodeJson(T value)
    {
        if (value == null)
            return 0;
        return JsonSerializer.Serialize(value).GetHashCode();
    }

    private static T GetSnapshotJson(T value)
    {
        if (value == null)
            return null!;
        var json = JsonSerializer.Serialize(value);
        return JsonSerializer.Deserialize<T>(json)!;
    }

    public JsonValueComparer() : base(
        (l, r) => EqualsJson(l, r),
        v => GetHashCodeJson(v),
        v => GetSnapshotJson(v))
    {
    }
}