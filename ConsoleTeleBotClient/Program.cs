using ConsoleTeleBot;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Services;

IConfiguration GetConfiguration(string fileName)
{
    return new ConfigurationBuilder()
        .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
        .AddJsonFile(fileName)
        .Build();
}

var appSettingsConfiguration = GetConfiguration("appsettings.json");
var configService = new ConfigService();
var settingsConfiguration = await configService.GetAppConfig();

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton(settingsConfiguration);
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(appSettingsConfiguration.GetConnectionString("DbConnection")));
builder.Services.AddHangfire(config => config.UseInMemoryStorage());
builder.Services.AddHangfireServer();
var app = builder.Build();

using var scope = app.Services.CreateScope();
var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
using var loggerFactory = LoggerFactory.Create(loggingBuilder =>
{
    loggingBuilder.AddConsole();
});
var logger = loggerFactory.CreateLogger<StartBot>();
var startBot = new StartBot(context, settingsConfiguration, logger);

app.UseHangfireDashboard("", new DashboardOptions {
    DashboardTitle = "ConsoleTeleBot",
    Authorization = new[] { new HangfireOpenAuthorizationFilter() }
});

await BotRun.Execute(settingsConfiguration, startBot, app);

app.Run();