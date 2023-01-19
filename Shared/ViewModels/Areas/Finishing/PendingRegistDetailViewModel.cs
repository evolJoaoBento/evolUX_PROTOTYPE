using Shared.Models.Areas.Finishing;

namespace Shared.ViewModels.Areas.Finishing
{
    public class PendingRegistDetailViewModel
    {
        public PendingRegistDetailInfo PendingRegistDetail { get; set; }
        public PendingRegistDetailViewModel()
        {
            PendingRegistDetail = new PendingRegistDetailInfo();
        }
    }
}
