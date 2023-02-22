using Shared.Models.Areas.Core;
using Shared.Models.Areas.Finishing;

namespace Shared.ViewModels.Areas.Finishing
{
    public class ProductionReportViewModel
    {
        public int ServiceCompanyID { get; set; }
        public string ServiceCompanyCode { get; set; }
        public string ServiceCompanyName { get; set; }
        public IEnumerable<ProdExpeditionElement> ProductionReport { get; set; }
        public IEnumerable<PrinterInfo> Printers { get; set; }
        public IEnumerable<ProductionDetailInfo> filters { get; set; }
    }
}
