using Shared.Models.Areas.Core;

namespace Shared.Models.Areas.Finishing
{
    public class PendingRecoverDetailInfo
    {
        public List<PendingRecoverElement> PendingRecoverFiles { get; set; }
        public List<PendingRecoverElement> PendingRecoverRegistDetailFiles { get; set; }
        public List<Job> PendingRecoverFilesJobs { get; set; }
        public List<Job> PendingRecoverRegistDetailFilesJobs { get; set; }
        public PendingRecoverDetailInfo()
        {
            PendingRecoverFiles = new List<PendingRecoverElement>();
            PendingRecoverRegistDetailFiles = new List<PendingRecoverElement>();
            PendingRecoverFilesJobs = new List<Job>();
            PendingRecoverRegistDetailFilesJobs = new List<Job>();
        }
    }
}
