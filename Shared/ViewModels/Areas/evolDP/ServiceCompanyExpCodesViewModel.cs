using Shared.Models.Areas.Core;
using Shared.Models.Areas.evolDP;

namespace Shared.ViewModels.Areas.evolDP
{
    public class ServiceCompanyExpCodesViewModel : ItemPermissions
    {
        public IEnumerable<ServiceTask> ServiceTasksList { get; set; }
    }
}
