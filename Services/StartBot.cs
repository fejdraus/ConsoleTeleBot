using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Services;

using System.Diagnostics;
using Telegram.Bot;

public class StartBot
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly string _botToken;
    private readonly string _appSettingPath;
    private readonly string _workingDirectory;
    private readonly string _arguments;
    private string _chatId;

    public StartBot(ApplicationDbContext applicationDbContext, AppConfig appConfig)
    {
        _applicationDbContext = applicationDbContext;
        _botToken = appConfig.BotConfig.Token;
        _appSettingPath = appConfig.AppSettings.AppPath;
        _workingDirectory = appConfig.AppSettings.WorkingDirectory;
        _arguments = appConfig.AppSettings.Arguments;
        _chatId = appConfig.BotConfig.ChatId;
    }
    public async Task Execute()
    {
        using CancellationTokenSource cts = new ();
        if (!string.IsNullOrWhiteSpace(_appSettingPath) && !string.IsNullOrWhiteSpace(_workingDirectory))
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = _appSettingPath,
                    Arguments = _arguments,
                    WorkingDirectory = _workingDirectory,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                }
            };

            process.Start();
    
            var service = new Service();
            var queryProcessing = _applicationDbContext.ParsingRules.ToList();
            _chatId = string.IsNullOrWhiteSpace(_botToken) ? "" : _chatId;
            var botClient = new TelegramBotClient(_botToken);
            try {
                if (string.IsNullOrWhiteSpace(_botToken))
                {
                    _chatId = "";
                }
                else
                {
                    var me = await botClient.GetMeAsync(cancellationToken: cts.Token);
                    Console.WriteLine($"Start bot @{me.Username}");
                }
                while (!process.StandardOutput.EndOfStream)
                {
                    await service.ParseAndSend(process, queryProcessing, botClient, _chatId, cts);
                }
                await process.WaitForExitAsync(cts.Token);
            } catch (Exception ex) {
                Console.WriteLine("Problem with Bot: " + ex.Message);
                cts.Cancel();
            }
        }
    }
}