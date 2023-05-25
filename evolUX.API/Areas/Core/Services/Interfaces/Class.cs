using System;
using System.IO;
using evolUX.API.Areas.Core.Services.Interfaces;

namespace evolUX.API.Areas.Core.Services
{
    public class Class : ILoggerService
    {
        string username = Environment.UserName; // Obtém o nome do usuário atual do Windows

        // Define o caminho completo para o arquivo de logs
        string filePath = "InternalLogs/logs.txt";

        public void LogType(string type, string message)
        {
            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine(type + ": " + message + " - " + username); //Não usar o username no controller(Enviroment.UserName)
            }
        }

        public void LogDebug(string message)
        {
            LogType("Debug", message);
        }

        public void LogError(string message)
        {
            LogType("Error", message);
        }

        public void LogInfo(string message)
        {
            LogType("Info", message);
        }

        public void LogWarn(string message)
        {
            LogType("Warning", message);
        }
    }
}
