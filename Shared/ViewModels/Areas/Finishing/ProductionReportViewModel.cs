using Shared.Models.Areas.Core;
using Shared.Models.Areas.Finishing;

namespace Shared.ViewModels.Areas.Finishing
{
    public class ProductionReportViewModel
    {
        public IEnumerable<ProductionDetailInfo> ProductionReport { get; set; }
        public IEnumerable<PrinterInfo> Printers { get; set; }
    }
}
