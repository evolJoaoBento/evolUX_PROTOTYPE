using Shared.Models.Areas.Core;
using Shared.Models.Areas.evolDP;

namespace Shared.ViewModels.Areas.evolDP
{
    public class ServiceTaskViewModel : ItemPermissions
    {
        public IEnumerable<ServiceTask> ServiceTasks { get; set; }
        public IEnumerable<ServiceElement> Services { get; set; }
    }
}
