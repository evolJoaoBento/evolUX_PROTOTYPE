using System;
using evolUX.API.Areas.Core.Services.Interfaces;
using NLog;

namespace evolUX.API.Areas.Core.Services.Interfaces
{
    public class LoggerService : ILoggerService
    {
        private static string logFilePath = "logs.txt";

        public void LogDebug(string message)
        {
            WriteToLogFile(message);
        }

        public void LogError(string message)
        {
            WriteToLogFile(message);
        }

        public void LogInfo(string message)
        {
            WriteToLogFile(message);
        }

        public void LogWarn(string message)
        {
            WriteToLogFile(message);
        }

        private void WriteToLogFile(string message)
        {
            try
            {
                System.IO.File.AppendAllText(logFilePath, $"{DateTime.Now:dd/MM/yyyy HH:mm:ss} - {message}{Environment.NewLine}");
            }
            catch (Exception ex)
            {
                // Tratar a exceção, se necessário
                // Por exemplo: logger.Error($"Falha ao gravar no arquivo de log: {ex.Message}");
            }
        }
    }
}
