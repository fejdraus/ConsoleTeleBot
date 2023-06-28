namespace Services;

using System.Diagnostics;
using Telegram.Bot;

public class StartBot
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly string _botToken;
    private readonly string _appSettingPath;
    private readonly string _chatId;

    public StartBot(ApplicationDbContext applicationDbContext, AppConfig appConfig)
    {
        _applicationDbContext = applicationDbContext;
        _botToken = appConfig.BotConfig.Token;
        _appSettingPath = appConfig.AppSettings.AppPath;
        _chatId = appConfig.BotConfig.ChatId;
    }
    public async Task Execute()
    {
        using CancellationTokenSource cts = new ();
        if (!string.IsNullOrWhiteSpace(_botToken) && !string.IsNullOrWhiteSpace(_appSettingPath) && !string.IsNullOrWhiteSpace(_chatId))
        {
            var botClient = new TelegramBotClient(_botToken);
            var appDirectory = Path.GetDirectoryName(_appSettingPath);

            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = _appSettingPath,
                    WorkingDirectory = appDirectory,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                }
            };

            process.Start();
    
            var service = new Service();
            var queryProcessing = _applicationDbContext.ParsingRules.ToList();
            while (!process.StandardOutput.EndOfStream)
            {
                await service.ParseAndSend(process, queryProcessing, botClient, _chatId, cts);
            }
            await process.WaitForExitAsync(cts.Token);
        }
    }
}