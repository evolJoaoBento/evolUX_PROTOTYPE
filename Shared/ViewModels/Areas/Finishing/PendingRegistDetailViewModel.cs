using Shared.Models.Areas.Core;
using Shared.Models.Areas.Finishing;

namespace Shared.ViewModels.Areas.Finishing
{
    public class PendingRegistDetailViewModel: ItemPermissions
    {
        public PendingRegistDetailInfo PendingRegistDetail { get; set; }
        public PendingRegistDetailViewModel()
        {
            PendingRegistDetail = new PendingRegistDetailInfo();
        }
    }
}
