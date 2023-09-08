using Shared.Models.Areas.Core;
using Shared.Models.Areas.Reports;

namespace Shared.ViewModels.Areas.Reports
{
    public class RetentionInfoReportViewModel: ItemPermissions
    {
        public int FileID { get; set; }
        public int RunID { get; set; }
        public RetentionInfoInfo RetentionInfo { get; set; }
    }
}
