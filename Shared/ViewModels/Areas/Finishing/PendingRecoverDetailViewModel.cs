using Shared.Models.Areas.Finishing;

namespace Shared.ViewModels.Areas.Finishing
{
    public class PendingRecoverDetailViewModel
    {
        public PendingRecoverDetailInfo PendingRecoverDetail { get; set; }
        public PendingRecoverDetailViewModel()
        {
            PendingRecoverDetail = new PendingRecoverDetailInfo();
        }
    }
}
