namespace Shared.Models.Areas.Finishing
{
    public class ProductionRunInfo
    {
        public int RunID { get; set; }
        public int ServiceCompanyID { get; set; }
        public string ServiceCompany { get; set; }
        public int BusinessID { get; set; }
        public string BusinessDesc { get; set; }
        public int RunDate { get; set; }
        public int RunSequence { get; set; }
        public int FilesLeftToPrint { get; set; }
        public int RecFilesLeftToPrint { get; set; }
        public int TotalProcessed { get; set; }
        public DateTime StartProcessed { get; set; }
        public DateTime EndProcessed { get; set; }
        public int TotalToPrint { get; set; }
        public DateTime StartToPrint { get; set; }
        public DateTime EndToPrint { get; set; }
        public int TotalS2Printer { get; set; }
        public DateTime StartS2Printer { get; set; }
        public DateTime EndS2Printer { get; set; }
        public int TotalPrinted { get; set; }
        public DateTime StartPrinted { get; set; }
        public DateTime EndPrinted { get; set; }
        public int TotalFullFill { get; set; }
        public DateTime StartFullFill { get; set; }
        public DateTime EndFullFill { get; set; }
        public int TotalExpedition { get; set; }
        public DateTime StartExpedition { get; set; }
        public DateTime EndExpedition { get; set; }
    }
}
