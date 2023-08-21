using System.ComponentModel.DataAnnotations;
using FluentValidation;

namespace ConsoleTeleBotWizard.Data;

public class AppConfigView : ICloneable
{
    public string BotConfigToken { get; set; } = "";
    public string BotConfigChatId { get; set; } = "";
    public string AppSettingsAppPath { get; set; } = "";
    public string AppSettingsWorkingDirectory { get; set; } = "";
    
    public string AppSettingsArguments { get; set; } = "";
    
    public string SchedulerCronExpression { get; set; } = "";

    public DateOnly? SchedulerStartDate { get; set; }
    
    public TimeOnly? SchedulerStartTime { get; set; }

    public object Clone()
    {
        return new AppConfigView
        {
            BotConfigToken = this.BotConfigToken,
            BotConfigChatId = this.BotConfigChatId,
            AppSettingsAppPath = this.AppSettingsAppPath,
            AppSettingsWorkingDirectory = this.AppSettingsWorkingDirectory,
            AppSettingsArguments = this.AppSettingsArguments,
            SchedulerCronExpression = this.SchedulerCronExpression,
            SchedulerStartDate = this.SchedulerStartDate,
            SchedulerStartTime = this.SchedulerStartTime
        };
    }
}

public class AppConfigViewValidator : AbstractValidator<AppConfigView>
{
    public AppConfigViewValidator()
    {
        RuleFor(appConfigView => appConfigView.SchedulerStartDate).Must((appConfigView, dateOnly) => dateOnly == null && appConfigView.SchedulerStartTime == null || dateOnly != null && appConfigView.SchedulerStartTime != null).WithMessage("The \"Date start\" and \"Time start\" fields must be filled in together.");
        RuleFor(appConfigView => appConfigView.SchedulerStartTime).Must((appConfigView, timeOnly) => timeOnly == null && appConfigView.SchedulerStartDate == null || timeOnly != null && appConfigView.SchedulerStartDate != null).WithMessage("The \"Date start\" and \"Time start\" fields must be filled in together.");
        RuleFor(appConfigView => appConfigView.BotConfigChatId).Must((appConfigView, chatId) => string.IsNullOrWhiteSpace(chatId) && string.IsNullOrWhiteSpace(appConfigView.BotConfigToken) || !string.IsNullOrWhiteSpace(chatId) && !string.IsNullOrWhiteSpace(appConfigView.BotConfigToken)).WithMessage("The \"Telegram bot token\" and \"Telegram contact Id or group Id\" fields must be filled in together.");
        RuleFor(appConfigView => appConfigView.BotConfigToken).Must((appConfigView, token) => string.IsNullOrWhiteSpace(token) && string.IsNullOrWhiteSpace(appConfigView.BotConfigChatId) || !string.IsNullOrWhiteSpace(token) && !string.IsNullOrWhiteSpace(appConfigView.BotConfigChatId)).WithMessage("The \"Telegram bot token\" and \"Telegram contact Id or group Id\" fields must be filled in together.");
        RuleFor(appConfigView => appConfigView.AppSettingsAppPath).NotEmpty().WithMessage("The \"Program or script\" field must be filled in.");
        RuleFor(appConfigView => appConfigView.AppSettingsWorkingDirectory).NotEmpty().WithMessage("The \"Working directory\" field must be filled in.");
    }
}