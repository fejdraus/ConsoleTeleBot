using Telegram.Bot;

namespace Services;

public class Service
{
    public async Task ParseAndSend(string line, List<ParsingRule> queryProcessing, TelegramBotClient? telegramBotClient,
        string? chatId, CancellationToken cancellationToken)
    {
        try
        {
            Console.WriteLine(line);
            if (!string.IsNullOrWhiteSpace(chatId) && telegramBotClient != null)
            {
                var queryProcessingRecord = queryProcessing.FirstOrDefault(qp => !string.IsNullOrWhiteSpace(line) && qp.CompiledRegex.IsMatch(line));

                if (queryProcessingRecord?.CompiledRegex != null && !string.IsNullOrWhiteSpace(queryProcessingRecord.Result))
                {
                    var result = queryProcessingRecord.CompiledRegex.Replace(line, queryProcessingRecord.Result);
                    if (!string.IsNullOrWhiteSpace(result))
                    {
                        await telegramBotClient.SendMessage(chatId, result,
                            disableNotification: queryProcessingRecord.QuietMessage,
                            cancellationToken: cancellationToken);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}