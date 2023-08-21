using AutoMapper;
using ConsoleTeleBotWizard.Data;
using Services;

namespace ConsoleTeleBotWizard.AutoMapperProfiles;

public class AppConfigProfile : Profile
{
    public AppConfigProfile()
    {
        CreateMap<AppConfig, AppConfigView>()
            .ForPath(dest => dest.BotConfigToken, opt => opt.MapFrom(c => c.BotConfig.Token))
            .ForPath(dest => dest.BotConfigChatId, opt => opt.MapFrom(c => c.BotConfig.ChatId))
            .ForPath(dest => dest.AppSettingsAppPath, opt => opt.MapFrom(c => c.AppSettings.AppPath))
            .ForPath(dest => dest.AppSettingsArguments, opt => opt.MapFrom(c => c.AppSettings.Arguments))
            .ForPath(dest => dest.AppSettingsWorkingDirectory, opt => opt.MapFrom(c => c.AppSettings.WorkingDirectory))
            .ForPath(dest => dest.SchedulerCronExpression, opt => opt.MapFrom(c => c.Scheduler.CronExpression))
            .ForPath(dest => dest.SchedulerStartDate, opt => opt.MapFrom(c => c.Scheduler.StartDate.HasValue ? DateOnly.FromDateTime(c.Scheduler.StartDate.Value.Date) : (DateOnly?)null))
            .ForPath(dest => dest.SchedulerStartTime, opt => opt.MapFrom(c => c.Scheduler.StartDate.HasValue ? TimeOnly.FromTimeSpan(c.Scheduler.StartDate.Value.TimeOfDay) : (TimeOnly?)null));
    }
}