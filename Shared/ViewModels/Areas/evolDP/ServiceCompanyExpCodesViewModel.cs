using Shared.Models.Areas.Core;
using Shared.Models.Areas.evolDP;

namespace Shared.ViewModels.Areas.evolDP
{
    public class ServiceCompanyExpCodesViewModel : ItemPermissions
    {
        public Company ServiceCompany { get; set; }
        public IEnumerable<ServiceCompanyExpCodeElement> ServiceCompanyExpCodes { get; set; }
        public IEnumerable<ExpCodeElement> ExpCodes { get; set; }
    }
}
