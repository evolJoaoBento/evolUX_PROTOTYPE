using Shared.Models.Areas.Core;
using Shared.Models.Areas.evolDP;

namespace Shared.ViewModels.Areas.evolDP
{
    public class ServiceWorkFlowViewModel : ItemPermissions
    {
        public IEnumerable<ServiceTaskElement> ServiceTasksList { get; set; }
        public IEnumerable<ServiceTypeElement> ServiceTypesList { get; set; }
    }
}
