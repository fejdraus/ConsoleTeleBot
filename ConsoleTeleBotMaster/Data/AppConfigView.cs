﻿using System.ComponentModel.DataAnnotations;

namespace ConsoleTeleBotMaster.Data;

public class AppConfigView : ICloneable
{
    public string BotConfigToken { get; set; } = "";
    public string BotConfigChatId { get; set; } = "";
    
    [Required(ErrorMessage = "The \"Program\" field is required.")]
    public string AppSettingsAppPath { get; set; } = "";
    
    [Required(ErrorMessage = "The \"Working directory\" field is required.")]
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