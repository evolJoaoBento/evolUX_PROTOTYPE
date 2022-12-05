namespace Shared.Models.Areas.Finishing
{
    public class PendingRegistry
    {
        public int RunID { get; set; }
        public int BusinessID { get; set; }
        public string BusinessDesc { get; set; }
        public int RunDate { get; set; }
        public int RunSequence { get; set; }
        public int TotalProcessed { get; set; }
        public int FilesLeftToPrint { get; set; }
        public int FilesLeftToRegistPrint { get; set; }
        public int FilesLeftToRegistFullFill { get; set; }
    }
}
