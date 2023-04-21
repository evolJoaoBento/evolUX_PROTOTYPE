using Shared.Models.Areas.Core;
using Shared.Models.Areas.evolDP;

namespace Shared.ViewModels.Areas.evolDP
{
    public class ServiceCompanyExpCodeConfigViewModel : ItemPermissions
    {
        public ExpCodeElement ExpCode { get; set; }
        public Company ServiceCompany { get; set; }
        public string ExpCenterCode { get; set; }
        public IEnumerable<ServiceCompanyExpCodeConfig> Configs { get; set; } = Enumerable.Empty<ServiceCompanyExpCodeConfig>();
        public IEnumerable<FulfillMaterialCode> FulfillMaterialCodes { get; set; }
    }
}
