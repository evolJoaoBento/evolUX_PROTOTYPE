namespace evolUX.UI.Areas.Core.Models
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }
        public ErrorResult? errorResult { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}