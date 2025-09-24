using System.Text.Json;

namespace Services;

public class ConfigService : IConfigService
{
    public async Task<AppConfig> GetAppConfig()
    {
        var configFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ConsoleTeleBotSettings.json");
        var options = new JsonSerializerOptions
        {
            TypeInfoResolver = AppConfigJsonContext.Default
        };
        options.Converters.Add(new CustomDateTimeConverter());
        if (!File.Exists(configFilePath))
        {
            var initialConfig = new AppConfig();
            var initialConfigJson = JsonSerializer.Serialize(initialConfig, options);
            await File.WriteAllTextAsync(configFilePath, initialConfigJson);
        }
        var configJson = await File.ReadAllTextAsync(configFilePath);
        var appConfig = JsonSerializer.Deserialize<AppConfig>(configJson, options);
        return appConfig ?? new AppConfig();
    }
}