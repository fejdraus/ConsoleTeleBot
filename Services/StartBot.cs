using System.Diagnostics;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using Telegram.Bot;

namespace Services;

public partial class StartBot(ApplicationDbContext applicationDbContext, AppConfig appConfig, ILogger<StartBot> logger)
{
    private readonly ApplicationDbContext _applicationDbContext = applicationDbContext ?? throw new ArgumentNullException(nameof(applicationDbContext));
    private readonly AppConfig _appConfig = appConfig ?? throw new ArgumentNullException(nameof(appConfig));
    private readonly ILogger<StartBot> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private TelegramBotClient? _botClient = null;
    public async Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(_appConfig.AppSettings.AppPath))
        {
            _logger.LogError("AppPath is not set.");
            return;
        }

        using var process = CreateProcess();
        process.Start();

        var service = new Service();
        var queryProcessing = _applicationDbContext.ParsingRules.ToList();
        

        try
        {
            if (!string.IsNullOrWhiteSpace(_appConfig.BotConfig.Token))
            {
                _botClient = new TelegramBotClient(_appConfig.BotConfig.Token);
                await InitializeBotAsync(cancellationToken);
            }
            
            await ProcessOutputAsync(process, service, queryProcessing, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Problem with Bot: {Message}", ex.Message);
        }
        finally
        {
            await process.WaitForExitAsync(cancellationToken);
        }
    }

    private Process CreateProcess()
    {
        return new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = _appConfig.AppSettings.AppPath,
                Arguments = FormatArguments(_appConfig.AppSettings.Arguments),
                WorkingDirectory = string.IsNullOrWhiteSpace(_appConfig.AppSettings.WorkingDirectory) ? 
                    AppContext.BaseDirectory : _appConfig.AppSettings.WorkingDirectory,
                RedirectStandardOutput = true,
                UseShellExecute = false,
            }
        };
    }

    private async Task InitializeBotAsync(CancellationToken cancellationToken)
    {
        if (_botClient != null && !string.IsNullOrWhiteSpace(_appConfig.BotConfig.Token))
        {
            var me = await _botClient.GetMe(cancellationToken);
            _logger.LogInformation("Start bot @{Username}", me.Username);
        }
        else
        {
            _logger.LogWarning("Bot token is not set.");
        }
    }

    private async Task ProcessOutputAsync(Process process, Service service, List<ParsingRule> queryProcessing, CancellationToken cancellationToken)
    {
        using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        while (!process.StandardOutput.EndOfStream)
        {
            var line = await process.StandardOutput.ReadLineAsync(cts.Token);
            if (!string.IsNullOrWhiteSpace(line)) await service.ParseAndSend(line, queryProcessing, _botClient, _appConfig.BotConfig.ChatId, cts.Token);
        }
    }

    private string FormatArguments(IEnumerable<string> arguments)
    {
        return MyRegex().Replace(string.Join(" ", arguments).Trim(), " ");
    }

    [GeneratedRegex(@"\s+")]
    private static partial Regex MyRegex();
}
