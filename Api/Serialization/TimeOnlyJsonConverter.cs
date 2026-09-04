using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Api.Serialization;

public sealed class TimeOnlyJsonConverter : JsonConverter<TimeOnly>
{
    private static readonly string[] Formats = ["HH:mm", "HH:mm:ss", "H:mm", "H:mm:ss"];

    public override TimeOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var str = reader.GetString();
        if (string.IsNullOrWhiteSpace(str))
        {
            throw new JsonException("Value cannot be empty for TimeOnly.");
        }

        if (TimeOnly.TryParseExact(str, Formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out var time))
        {
            return time;
        }

        if (TimeOnly.TryParse(str, CultureInfo.InvariantCulture, out time))
        {
            return time;
        }

        throw new JsonException($"Unable to parse '{str}' to TimeOnly. Supported formats: HH:mm, HH:mm:ss.");
    }

    public override void Write(Utf8JsonWriter writer, TimeOnly value, JsonSerializerOptions options) =>
        writer.WriteStringValue(value.ToString("HH:mm", CultureInfo.InvariantCulture));
}

public sealed class NullableTimeOnlyJsonConverter : JsonConverter<TimeOnly?>
{
    private static readonly string[] Formats = ["HH:mm", "HH:mm:ss", "H:mm", "H:mm:ss"];

    public override TimeOnly? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var str = reader.GetString();
        if (string.IsNullOrWhiteSpace(str)) return null;

        if (TimeOnly.TryParseExact(str, Formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out var time))
        {
            return time;
        }

        if (TimeOnly.TryParse(str, CultureInfo.InvariantCulture, out time))
        {
            return time;
        }

        throw new JsonException($"Unable to parse '{str}' to TimeOnly. Supported formats: HH:mm, HH:mm:ss.");
    }

    public override void Write(Utf8JsonWriter writer, TimeOnly? value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }

        writer.WriteStringValue(value.Value.ToString("HH:mm", CultureInfo.InvariantCulture));
    }
}
