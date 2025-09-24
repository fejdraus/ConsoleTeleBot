using System.Text.Json.Serialization;

namespace Services;

public class LogLevel
{
    public string Default { get; set; } = "";
    // ReSharper disable once InconsistentNaming
    [JsonPropertyName("Microsoft.AspNetCore")]
    public string Microsoft_AspNetCore { get; set; } = "";
}