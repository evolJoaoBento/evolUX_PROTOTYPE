using Shared.Models.Areas.Core;
using Shared.Models.Areas.evolDP;

namespace Shared.ViewModels.Areas.evolDP
{
    public class ServiceTypeDetailViewModel : ItemPermissions
    {
        public ServiceTypeElement Type { get; set; }
        public IEnumerable<Company> ServiceCompanies { get; set; }
    }
}
