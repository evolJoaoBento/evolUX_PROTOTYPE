using Shared.Models.Areas.Core;
using Shared.Models.Areas.Finishing;

namespace Shared.ViewModels.Areas.Finishing
{
    public class PendingRegistViewModel: ItemPermissions
    {
        public IEnumerable<PendingRegistInfo> PendingRegist { get; set; }
    }
}
