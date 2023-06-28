using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Services;

IConfiguration GetConfiguration(string fileName)
{
    return new ConfigurationBuilder()
        .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
        .AddJsonFile(fileName)
        .Build();
}
IServiceProvider ConfigureService(IConfiguration configuration)
{
    var services = new ServiceCollection();
    services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlite(configuration.GetConnectionString("DbConnection")));
    return services.BuildServiceProvider();
}
var appSettingsConfiguration = GetConfiguration("appsettings.json");
var settingsConfiguration = GetConfiguration("ConsoleTeleBotSettings.json").Get<AppConfig>();
var serviceProvider = ConfigureService(appSettingsConfiguration);
using var scope = serviceProvider.CreateScope();
var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
var startBot = new StartBot(context, settingsConfiguration);
await startBot.Execute();