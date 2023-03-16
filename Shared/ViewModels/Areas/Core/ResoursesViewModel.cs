using Shared.Models.Areas.Core;

namespace Shared.ViewModels.Areas.Core
{
    public class ResoursesViewModel: ItemPermissions
    {
        public IEnumerable<ResourceInfo> Resources { get; set; }
    }
}
