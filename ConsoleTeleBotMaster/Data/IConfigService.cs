using System.Threading.Tasks;
using Services;

namespace ConsoleTeleBotMaster.Data;

public interface IConfigService
{
    Task<AppConfig> GetAppConfig();
}