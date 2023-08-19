using System.Text.Json;
using System.Text.Json.Serialization;

namespace Services;

public class DateOnlyConverter : JsonConverter<DateOnly>
{
    public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            if (DateOnly.TryParse(reader.GetString(), out DateOnly date))
            {
                return date;
            }
        }

        throw new JsonException($"Cannot convert {reader.TokenType} to {nameof(DateOnly)}.");
    }

    public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}