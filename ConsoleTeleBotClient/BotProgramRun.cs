using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Services;

namespace ConsoleTeleBot;

public static class BotRun
{
    public static async Task Execute(AppConfig appConfig, StartBot startBot, WebApplication webApplication)
    {
        if (string.IsNullOrWhiteSpace(appConfig.Scheduler.CronExpression) && appConfig.Scheduler.StartDate is null)
        {
            await startBot.Execute();
        }
        else
        {
            webApplication.Services.GetRequiredService<IHostApplicationLifetime>().ApplicationStarted.Register(() =>
            {
                if (!string.IsNullOrWhiteSpace(appConfig.Scheduler.CronExpression))
                {
                    var recurringJobManager = webApplication.Services.GetRequiredService<IRecurringJobManager>();
                    recurringJobManager.AddOrUpdate("StartBot", () => startBot.Execute(),
                        appConfig.Scheduler.CronExpression, new RecurringJobOptions
                        {
                            TimeZone = TimeZoneInfo.Local
                        });
                }

                if (appConfig.Scheduler.StartDate is not null)
                {
                    var startDateTimeString = appConfig.Scheduler.StartDate ?? DateTime.MinValue;
                    if(startDateTimeString >= DateTime.Now)
                        BackgroundJob.Schedule(() => startBot.Execute(), startDateTimeString);
                }
            });
        }
    }
}