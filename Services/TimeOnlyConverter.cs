using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Services;

public class TimeOnlyConverter : JsonConverter<TimeOnly>
{
    public override TimeOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            if (TimeOnly.TryParse(reader.GetString(), out TimeOnly time))
            {
                return time;
            }
        }

        throw new JsonException($"Cannot convert {reader.TokenType} to {nameof(TimeOnly)}.");
    }

    public override void Write(Utf8JsonWriter writer, TimeOnly value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}