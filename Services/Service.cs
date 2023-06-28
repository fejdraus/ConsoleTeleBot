using System.Diagnostics;
using System.Text.RegularExpressions;
using Telegram.Bot;

namespace Services;

public class Service
{
    public async Task ParseAndSend(Process process1, List<ParsingRule> queryProcessings, TelegramBotClient telegramBotClient,
        string? chatId, CancellationTokenSource cancellationTokenSource)
    {
        var line = process1.StandardOutput.ReadLine();
        Console.WriteLine(line);
        if (line is not null)
        {
            var queryProcessingRecord = queryProcessings.FirstOrDefault(qp => qp.ConsoleOutput != null && line.Contains(qp.ConsoleOutput));

            if (queryProcessingRecord?.RegexPattern != null)
            {
                var match = Regex.Match(line, queryProcessingRecord.RegexPattern);
                if (match.Success)
                {
                    var replacements = new Dictionary<string, string>();
                    for (int i = 1; i < match.Groups.Count; i++)
                    {
                        replacements.Add($"{{m.Groups[{i}].Value}}", match.Groups[i].Value);
                    }

                    var result = queryProcessingRecord.Result;
                    foreach (var replacement in replacements)
                    {
                        result = result?.Replace(replacement.Key, replacement.Value);
                    }

                    if (!string.IsNullOrWhiteSpace(result))
                    {
                        if (chatId != null)
                            await telegramBotClient.SendTextMessageAsync(chatId, result,
                                disableNotification: queryProcessingRecord.QuietMessage,
                                cancellationToken: cancellationTokenSource.Token);
                    }
                }
            }
        }
    }
}