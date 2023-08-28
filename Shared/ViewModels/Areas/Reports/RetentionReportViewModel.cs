using Shared.Models.Areas.Core;
using Shared.Models.Areas.Finishing;

namespace Shared.ViewModels.Areas.Reports
{
    public class RetentionReportViewModel: ItemPermissions
    {
        public int BusinessAreaID { get; set; }
        public string BusinessAreaCode { get; set; }
        public string BusinessAreaName { get; set; }
        public IEnumerable<ProdExpeditionElement> RetentionReport { get; set; }
        public IEnumerable<PrinterInfo> Printers { get; set; }
        public IEnumerable<ProductionDetailInfo> filters { get; set; }
    }
}
