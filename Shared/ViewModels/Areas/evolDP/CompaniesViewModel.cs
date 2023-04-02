using Shared.Models.Areas.Core;
using Shared.Models.Areas.evolDP;

namespace Shared.ViewModels.Areas.evolDP
{
    public class CompaniesViewModel : ItemPermissions
    {
        public IEnumerable<ExpeditionTypeElement> Types { get; set; }
        public IEnumerable<Company> Companies { get; set; }
        public IEnumerable<Business> CompanyBusiness { get; set; }
    }
}
