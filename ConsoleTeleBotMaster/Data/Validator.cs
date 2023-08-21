using FluentValidation;

namespace ConsoleTeleBotMaster.Data;


public class Validator : AbstractValidator<AppConfigView>
{
    public Validator()
    {
        RuleFor(appConfigView => appConfigView.SchedulerStartDate).Must((appConfigView, dateOnly) => dateOnly == null && appConfigView.SchedulerStartTime == null || dateOnly != null && appConfigView.SchedulerStartTime != null).WithMessage("The \"Date start\" and \"Time start\" fields must be filled in together.");
        RuleFor(appConfigView => appConfigView.SchedulerStartTime).Must((appConfigView, timeOnly) => timeOnly == null && appConfigView.SchedulerStartDate == null || timeOnly != null && appConfigView.SchedulerStartDate != null).WithMessage("The \"Date start\" and \"Time start\" fields must be filled in together.");
        RuleFor(appConfigView => appConfigView.BotConfigChatId).Must((appConfigView, chatId) => string.IsNullOrWhiteSpace(chatId) && string.IsNullOrWhiteSpace(appConfigView.BotConfigToken) || !string.IsNullOrWhiteSpace(chatId) && !string.IsNullOrWhiteSpace(appConfigView.BotConfigToken)).WithMessage("The \"Telegram bot token\" and \"Telegram contact Id or group Id\" fields must be filled in together.");
        RuleFor(appConfigView => appConfigView.BotConfigToken).Must((appConfigView, token) => string.IsNullOrWhiteSpace(token) && string.IsNullOrWhiteSpace(appConfigView.BotConfigChatId) || !string.IsNullOrWhiteSpace(token) && !string.IsNullOrWhiteSpace(appConfigView.BotConfigChatId)).WithMessage("The \"Telegram bot token\" and \"Telegram contact Id or group Id\" fields must be filled in together.");
        RuleFor(appConfigView => appConfigView.AppSettingsAppPath).NotEmpty().WithMessage("The \"Program or script\" field must be filled in.");
        RuleFor(appConfigView => appConfigView.AppSettingsWorkingDirectory).NotEmpty().WithMessage("The \"Working directory\" field must be filled in.");
    }
}