namespace Shared.Models.Areas.Finishing
{
    public class PendingRecoverElement
    {
        public int RunID { get; set; }
        public string FileName { get; set; }
        public int RecoverType { get; set; }
        public int StartPostObjID { get; set; }
        public int EndPostObjID { get; set; }
        public string UserName { get; set; }
    }
}
