using Shared.Models.Areas.Core;
using Shared.Models.Areas.Finishing;

namespace Shared.ViewModels.Areas.Finishing
{
    public class ProductionRunReportViewModel: ItemPermissions
    {
        public IEnumerable<ProductionRunInfo> ProductionRunReport { get; set; }
    }
}
