using Shared.Models.Areas.Core;
using Shared.Models.Areas.evolDP;

namespace Shared.ViewModels.Areas.Finishing
{
    public class ServiceCompanyViewModel: ItemPermissions
    {
        public IEnumerable<Company> ServiceCompanies { get; set; }
    }
}
