using Shared.Models.Areas.Core;
using Shared.Models.Areas.Reports;

namespace Shared.ViewModels.Areas.Reports
{
    public class RetentionInfoReportViewModel: ItemPermissions
    {
        public int BusinessAreaID { get; set; }
        public string BusinessAreaCode { get; set; }
        public string BusinessAreaName { get; set; }
        public IEnumerable<RetentionInfo> RetentionReport { get; set; }
    }
}
