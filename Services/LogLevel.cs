using Newtonsoft.Json;

namespace Services;

public class LogLevel
{
    public string Default { get; set; } = "";
    // ReSharper disable once InconsistentNaming
    [JsonProperty("Microsoft.AspNetCore")]
    public string Microsoft_AspNetCore { get; set; } = "";
}