using Shared.Models.Areas.Finishing;

namespace Shared.ViewModels.Areas.Finishing
{
    public class ProductionReportViewModel
    {
        public IEnumerable<ProductionDetailInfo> ProductionReport { get; set; }
        public IEnumerable<ResourceInfo> Resources { get; set; }
        public string ServiceCompanyCode { get; set; }
    }
}
