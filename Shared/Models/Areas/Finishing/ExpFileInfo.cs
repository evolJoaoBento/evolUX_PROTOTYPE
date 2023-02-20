using Shared.Models.Areas.evolDP;

namespace Shared.Models.Areas.Finishing
{
    public class ExpFileInfo : FileBase
    {
        public int TotalPostObjs { get; set; }
        public List<ExpLevelInfo> ExpCompanyLevels { get; set; }
        public ExpFileInfo()
        {
            ExpCompanyLevels = new List<ExpLevelInfo>();
        }

    }
}
