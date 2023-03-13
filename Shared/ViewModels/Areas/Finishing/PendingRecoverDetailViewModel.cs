using Shared.Models.Areas.Core;
using Shared.Models.Areas.Finishing;

namespace Shared.ViewModels.Areas.Finishing
{
    public class PendingRecoverDetailViewModel: ItemPermissions
    {
        public PendingRecoverDetailInfo PendingRecoverDetail { get; set; }
        public PendingRecoverDetailViewModel()
        {
            PendingRecoverDetail = new PendingRecoverDetailInfo();
        }
    }
}
