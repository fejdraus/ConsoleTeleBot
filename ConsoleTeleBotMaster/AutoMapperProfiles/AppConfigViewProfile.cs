using AutoMapper;
using ConsoleTeleBotMaster.Data;
using Services;

namespace ConsoleTeleBotMaster.AutoMapperProfiles;

public class AppConfigViewProfile : Profile
{
    public AppConfigViewProfile()
    {
        CreateMap<AppConfigView, AppConfig>()
            .ForPath(dest => dest.BotConfig.Token, opt => opt.MapFrom(c => c.BotConfigToken))
            .ForPath(dest => dest.BotConfig.ChatId, opt => opt.MapFrom(c => c.BotConfigChatId))
            .ForPath(dest => dest.AppSettings.AppPath, opt => opt.MapFrom(c => c.AppSettingsAppPath))
            .ForPath(dest => dest.AppSettings.Arguments, opt => opt.MapFrom(c => c.AppSettingsArguments))
            .ForPath(dest => dest.AppSettings.WorkingDirectory, opt => opt.MapFrom(c => c.AppSettingsWorkingDirectory))
            .ForPath(dest => dest.Scheduler.CronExpression, opt => opt.MapFrom(c => c.SchedulerCronExpression))
            .ForPath(dest => dest.Scheduler.StartDate, opt => opt.MapFrom(c => c.SchedulerStartDate.HasValue && c.SchedulerStartTime.HasValue ? c.SchedulerStartDate.Value.ToDateTime(c.SchedulerStartTime.Value) : (DateTime?)null));
    }
}