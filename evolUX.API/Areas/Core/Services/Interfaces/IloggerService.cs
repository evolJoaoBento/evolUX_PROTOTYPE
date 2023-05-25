using NLog;

namespace evolUX.API.Areas.Core.Services.Interfaces
{
    public interface ILoggerService
    {
        void LogDebug(string message);
        void LogError(string message);
        void LogInfo(string message);
        void LogWarn(string message);
    }
    
}