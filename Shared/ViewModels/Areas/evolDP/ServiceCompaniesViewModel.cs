using Shared.Models.Areas.Core;
using Shared.Models.Areas.evolDP;

namespace Shared.ViewModels.Areas.evolDP
{
    public class ServiceCompaniesViewModel : ItemPermissions
    {
        public IEnumerable<Company> ServiceCompanies { get; set; }
        public IEnumerable<ServiceCompanyRestriction> Restrictions { get; set; }
    }
}
