using NLog;

namespace evolUX.UI.Areas.Core.Services.Interfaces
{
    public interface ILoggerServices
    {
        void LogDebug(string message);
        void LogError(string message);
        void LogInfo(string message);
        void LogWarn(string message);
    }
    
}