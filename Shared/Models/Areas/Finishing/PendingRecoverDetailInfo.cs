namespace Shared.Models.Areas.Finishing
{
    public class PendingRecoverDetailInfo
    {
        public List<PendingRecoverElement> PendingRecoverFiles { get; set; }
        public List<PendingRecoverElement> PendingRecoverRegistDetailFiles { get; set; }
        public PendingRecoverDetailInfo()
        {
            PendingRecoverFiles = new List<PendingRecoverElement>();
            PendingRecoverRegistDetailFiles = new List<PendingRecoverElement>();
        }
    }
}
