using Shared.Models.Areas.Core;
using Shared.Models.Areas.evolDP;

namespace Shared.ViewModels.Areas.evolDP
{
    public class ServiceCompanyConfigViewModel : ItemPermissions
    {
        public Company ServiceCompany { get; set; }
        public int ServiceTypeID { get; set; }
        public int ServiceID { get; set; }
        public IEnumerable<ServiceCompanyService> Configs { get; set; }
    }
}
