namespace Shared.Models.Areas.Finishing
{
    public class PendingRegistDetailInfo
    {
        public List<PendingRegistElement> ToRegistPrintFiles { get; set; }
        public List<PendingRegistElement> ToRegistFullfillFiles { get; set; }
        public PendingRegistDetailInfo()
        {
            ToRegistPrintFiles = new List<PendingRegistElement>();
            ToRegistFullfillFiles = new List<PendingRegistElement>();
        }
    }
}
