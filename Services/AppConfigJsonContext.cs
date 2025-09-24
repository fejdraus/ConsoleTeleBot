using System.Text.Json.Serialization;

namespace Services;

[JsonSerializable(typeof(AppConfig))]
internal partial class AppConfigJsonContext : JsonSerializerContext
{
}