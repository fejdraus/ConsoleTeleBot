using System.Diagnostics;
using Telegram.Bot;
using ConsoleTeleBot;
using JsonSerializer = System.Text.Json.JsonSerializer;
using Microsoft.EntityFrameworkCore;

string configFile = File.ReadAllText("appsettings.json");

var config = JsonSerializer.Deserialize<AppConfig>(configFile);

using CancellationTokenSource cts = new ();
if (config?.BotConfig.Token != null)
{
    var botClient = new TelegramBotClient(config.BotConfig.Token);
    string appPath = config.AppSettings.AppPath;
    string? appDirectory = Path.GetDirectoryName(appPath);

    var process = new Process
    {
        StartInfo = new ProcessStartInfo
        {
            FileName = appPath,
            WorkingDirectory = appDirectory,
            RedirectStandardOutput = true,
            UseShellExecute = false,
        }
    };

    process.Start();
    
    var service = new Service();

    await using (var db = new ApplicationDbContext())
    {
        var queryProcessings = db.ParsingRules.ToList();
        while (!process.StandardOutput.EndOfStream)
        {
            await service.ParseAndSend(process, queryProcessings, botClient, config, cts);
        }
    }
    process.WaitForExit();
}