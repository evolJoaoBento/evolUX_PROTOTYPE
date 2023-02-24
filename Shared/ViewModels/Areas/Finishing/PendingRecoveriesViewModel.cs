using Shared.Models.Areas.Core;
using Shared.Models.Areas.Finishing;

namespace Shared.ViewModels.Areas.Finishing
{
    public class PendingRecoveriesViewModel: ItemPermissions
    {
        public IEnumerable<PendingRecoverElement> PendingRecoveries { get; set; }
    }
}
