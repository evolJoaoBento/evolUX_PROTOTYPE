using Shared.Models.Areas.Core;

namespace Shared.ViewModels.Areas.Core
{
    public class MessageViewModel
    {
        public string MessageID { get; set; }
        public string MessageHeader { get; set; }
        public string MessageDetail { get; set; }

        public MessageViewModel() 
        {
            MessageID = string.Empty;
            MessageHeader = string.Empty;
            MessageDetail = string.Empty;
        }
        public MessageViewModel(string message) 
        {
            MessageID = string.Empty;
            MessageHeader = string.Empty;
            MessageDetail = message;
        }
        public MessageViewModel(string messageHeader, string message)
        {
            MessageID = string.Empty;
            MessageHeader = messageHeader;
            MessageDetail = message;
        }
        public MessageViewModel(string messageID, string messageHeader, string message)
        {
            MessageID = messageID;
            MessageHeader = messageHeader;
            MessageDetail = message;
        }
    }
}