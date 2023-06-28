using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Services;

namespace ConsoleTeleBotMaster.Data;

public class ConfigService : IConfigService
{
    private readonly IWebHostEnvironment _env;

    public ConfigService(IWebHostEnvironment env)
    {
        _env = env;
    }

    public async Task<AppConfig> GetAppConfig()
    {
        var configFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ConsoleTeleBotSettings.json");
        if (!File.Exists(configFilePath))
        {
            var initialConfig = new AppConfig();
            var initialConfigJson = JsonSerializer.Serialize(initialConfig);
            await File.WriteAllTextAsync(configFilePath, initialConfigJson);
        }
        var configJson = await File.ReadAllTextAsync(configFilePath);
        var options = new JsonSerializerOptions{PropertyNamingPolicy = new PreservePropertyNames()};
        var appConfig = JsonSerializer.Deserialize<AppConfig>(configJson, options);
        return appConfig ?? new AppConfig();
    }
}