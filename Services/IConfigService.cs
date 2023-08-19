namespace Services;

public interface IConfigService
{
    Task<AppConfig> GetAppConfig();
}