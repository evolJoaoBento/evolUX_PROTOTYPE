using Shared.Models.Areas.Core;
namespace Shared.Models.Areas.Finishing
{
    public class PrinterInfo : ResourceInfo
    {
        public bool PrintColor { get; set; }
        public bool PrintBlack { get; set; }
        public int PlexFeature { get; set; }
    }
}
