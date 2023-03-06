using Shared.Models.Areas.Core;
using Shared.Models.Areas.evolDP;

namespace Shared.ViewModels.Areas.evolDP
{
    public class ServiceCompanyViewModel : ItemPermissions
    {
        public Company ServiceCompany { get; set; }
        public IEnumerable<ServiceCompanyServiceResume> Configs { get; set; }
        public IEnumerable<ServiceCompanyRestriction> Restrictions { get; set; }
    }
}
