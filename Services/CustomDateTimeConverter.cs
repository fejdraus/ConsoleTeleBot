using System.Text.Json;
using System.Text.Json.Serialization;

namespace Services;

public class CustomDateTimeConverter : JsonConverter<DateTime?>
{
    public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var dateStr = reader.GetString();
        if (string.IsNullOrEmpty(dateStr))
        {
            return null;
        }

        if (DateTime.TryParse(dateStr, out var dateTime))
        {
            return dateTime;
        }

        throw new JsonException($"Invalid date format: {dateStr}");
    }

    public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value?.ToString("O"));
    }
}