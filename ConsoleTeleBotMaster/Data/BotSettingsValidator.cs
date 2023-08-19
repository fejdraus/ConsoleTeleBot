using System.ComponentModel.DataAnnotations;
using Services;

namespace ConsoleTeleBotMaster.Data;

public class BotSettingsValidator : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (validationContext.ObjectInstance is AppConfigView botConfigInstance)
        {
            if (string.IsNullOrWhiteSpace(botConfigInstance.BotConfigToken) && !string.IsNullOrWhiteSpace(botConfigInstance.BotConfigChatId))
            {
                return new ValidationResult("The \"Telegram bot token\" fields are required if the \"Name Telegram contact or group\" field is filled.");
            }
        
            if (!string.IsNullOrWhiteSpace(botConfigInstance.BotConfigToken) && string.IsNullOrWhiteSpace(botConfigInstance.BotConfigChatId))
            {
                return new ValidationResult("The \"Name Telegram contact or group\" fields are required if the \"Telegram bot token\" field is filled.");
            }
        }
        
        return ValidationResult.Success;
    }
}