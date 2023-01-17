using Shared.ViewModels.Areas.Core;
using System.Runtime.Serialization;

namespace Shared.Exceptions
{
    [Serializable]
    public class ControledErrorException : Exception
    {
        public MessageViewModel ControledMessage { get; set; }
        public ControledErrorException() 
        {
            ControledMessage = new MessageViewModel();
        }
        public ControledErrorException(string message)
        {
            ControledMessage = new MessageViewModel(message);
        }
        public ControledErrorException(string messageHeader, string message)
        {
            ControledMessage = new MessageViewModel(messageHeader, message);
        }
        public ControledErrorException(string messageID, string messageHeader, string message)
        {
            ControledMessage = new MessageViewModel(messageID, messageHeader, message);
        }
    }
}