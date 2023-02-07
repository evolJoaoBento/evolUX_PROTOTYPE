using Shared.Models.Areas.Core;
using Shared.Models.Areas.Finishing;

namespace Shared.ViewModels.Areas.Finishing
{
    public class ProductionReportPrinterViewModel
    {
        public IEnumerable<ProdExpeditionElement> ProductionReport { get; set; }
        public IEnumerable<PrinterInfo> Printers { get; set; }
    }
}
