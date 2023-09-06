using Shared.Models.Areas.Core;
using Shared.Models.Areas.Reports;

namespace Shared.ViewModels.Areas.Reports
{
    public class RetentionRunReportViewModel: ItemPermissions
    {
        public IEnumerable<RetentionRunInfo> RetentionRunReport { get; set; }
        public int RefDate { get; set; }
    }
}
