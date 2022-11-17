using Shared.Models.Areas.Core;

namespace Shared.ViewModels.Areas.Core
{
    public class ErrorViewModel
    {
        public string RequestID { get; set; }
        public ErrorResult ErrorResult { get; set; }

        public ErrorViewModel() 
        { 
            ErrorResult = new ErrorResult();
        }
        public ErrorViewModel(string RequestID) 
        { 
            ErrorResult = new ErrorResult();
            this.RequestID = RequestID;
        }
    }
}