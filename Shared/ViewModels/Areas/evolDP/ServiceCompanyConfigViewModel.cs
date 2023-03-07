using Shared.Models.Areas.Core;
using Shared.Models.Areas.evolDP;

namespace Shared.ViewModels.Areas.evolDP
{
    public class ServiceCompanyConfigViewModel : ItemPermissions
    {
        public Company ServiceCompany { get; set; }
        public int CostDate { get; set; }
        public int ServiceID { get; set; }
        public IEnumerable<ServiceCompanyService> Configs { get; set; }
        public IEnumerable<ServiceElement> Services { get; set; }
    }
}
