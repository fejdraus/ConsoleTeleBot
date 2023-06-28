namespace Services;

public class AppConfig
{
    public BotConfig BotConfig { get; set; } = new();
    public AppSettings AppSettings { get; set; } = new();
}