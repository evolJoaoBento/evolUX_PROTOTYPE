using Shared.Models.Areas.Core;
using Shared.Models.Areas.evolDP;

namespace Shared.ViewModels.Areas.evolDP
{
    public class ServiceTypeViewModel : ItemPermissions
    {
        public IEnumerable<ServiceTypeElement> Types { get; set; }
        public IEnumerable<Company> ServiceCompanies { get; set; }
    }
}
