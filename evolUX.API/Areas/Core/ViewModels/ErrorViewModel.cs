using evolUX.API.Areas.Core.ViewModels;

namespace evolUX.API.Areas.Core.ViewModels
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }
        public ErrorResult? errorResult { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}